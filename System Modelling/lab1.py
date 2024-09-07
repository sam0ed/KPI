import numpy as np
import matplotlib.pyplot as plt
import scipy.stats as stats
import pandas as pd
# Set the seed for reproducibility
np.random.seed(42)

# Function to generate random numbers using method 1 (Exponential Distribution)
def method_1(lambd, size=10000):
    xi = np.random.uniform(0, 1, size)
    x = -1 / lambd * np.log(xi)
    return x

# Function to generate random numbers using method 2 (Normal Distribution approximation)
def method_2(a, sigma, size=10000):
    xi = np.random.uniform(0, 1, (size, 12))
    mu = np.sum(xi, axis=1) - 6
    x = sigma * mu + a
    return x

# Function to generate random numbers using method 3 (Linear Congruential Generator)
def method_3(a=5**13, c=2**31, size=10000):
    z = np.zeros(size)
    z[0] = np.random.randint(0, c)
    for i in range(1, size):
        z[i] = (a * z[i-1]) % c
    x = z / c
    return x

def calculate_mean_and_variance(data):
    mean = np.mean(data)  # Calculate the mean (середнє)
    variance = np.var(data)  # Calculate the variance (дисперсія)
    return mean, variance

def merge_bins(observed, expected, bins, min_count):
    new_observed = []
    new_expected = []
    new_bins = [bins[0]]
    
    current_observed = observed[0]
    current_expected = expected[0]
    
    for i in range(1, len(observed)):
        if current_observed < min_count:
            # Merge the current bin with the next one
            current_observed += observed[i]
            current_expected += expected[i]
        else:
            # If current bin has enough points, finalize it
            new_observed.append(current_observed)
            new_expected.append(current_expected)
            new_bins.append(bins[i])
            current_observed = observed[i]
            current_expected = expected[i]
    
    # Append the last bin if not merged
    new_observed.append(current_observed)
    new_expected.append(current_expected)
    new_bins.append(bins[-1])

    return np.array(new_observed), np.array(new_expected), np.array(new_bins)


# Function to plot histogram and perform chi-square test
def analyze_data(data, theoretical_dist, params, title):
    # Number of bins for the histogram
    NUM_BINS = 20
    # Minimum number of counts for each bin
    MIN_COUNT = 5
    
    # Plot the histogram of the data
    plt.figure(figsize=(8, 6))
    plt.hist(data, bins=NUM_BINS, density=True, alpha=0.6, color='g', label='Generated Data')
    
    # Plot the theoretical distribution
    x = np.linspace(min(data), max(data), 1000)
    if theoretical_dist == 'exponential':
        pdf = stats.expon.pdf(x, scale=1/params['lambda'])
        plt.plot(x, pdf, 'r-', label=f'Theoretical Exponential (λ={params["lambda"]})')
    elif theoretical_dist == 'normal':
        pdf = stats.norm.pdf(x, loc=params['mean'], scale=params['std'])
        plt.plot(x, pdf, 'r-', label=f'Theoretical Normal (μ={params["mean"]}, σ={params["std"]})')
    elif theoretical_dist == 'uniform':
        plt.axhline(1, color='r', label='Theoretical Uniform (0,1)')
    
    plt.legend()
    plt.title(title)
    plt.show()

    observed, bins = np.histogram(data, bins=NUM_BINS, density=False)
    expected = None
        
    # Compute the bin centers (this gives a more accurate representation than using bin edges)
    bin_centers = (bins[:-1] + bins[1:]) / 2

    # Now calculate the expected frequencies by scaling the PDF
    if theoretical_dist == 'exponential':
        expected = stats.expon.pdf(bin_centers, scale=1/params['lambda'])
    elif theoretical_dist == 'normal':
        expected = stats.norm.pdf(bin_centers, loc=params['mean'], scale=params['std'])
    elif theoretical_dist == 'uniform':
        expected = np.ones_like(observed) / len(observed) 
    # this multiplication by len(data) is unnecessary because we are not changing ratios of values inside the array, we are just scaling them
    # expected = expected * len(data) 
    expected = expected * (np.sum(observed) / np.sum(expected))
    
    # Merge bins with fewer than 5 observed counts
    observed, expected, bins = merge_bins(observed, expected, bins, min_count=MIN_COUNT)
    
    chi2_stat, p_value = stats.chisquare(observed, expected)
    return chi2_stat, p_value

def get_user_input(prompt, default_value):
    user_input = input(f"{prompt} [Default: {default_value}]: ")
    return float(user_input) if user_input else default_value


def main():
    # Prompt user for parameters of Method 1 (Exponential)
    lambd = get_user_input("Enter lambda for Exponential distribution", 1)
    data_1 = method_1(lambd)
    mean_1, variance_1 = calculate_mean_and_variance(data_1)
    print(f"Method 1 (Exponential) - Mean: {mean_1}, Variance: {variance_1}")
    chi2_stat_1, p_value_1 = analyze_data(data_1, 'exponential', {'lambda': lambd}, 'Exponential Distribution - Method 1')

    # Prompt user for parameters of Method 2 (Normal)
    a = get_user_input("Enter mean (a) for Normal distribution", 0)
    sigma = get_user_input("Enter standard deviation (sigma) for Normal distribution", 1)
    data_2 = method_2(a, sigma)
    mean_2, variance_2 = calculate_mean_and_variance(data_2)
    print(f"Method 2 (Normal) - Mean: {mean_2}, Variance: {variance_2}")
    chi2_stat_2, p_value_2 = analyze_data(data_2, 'normal', {'mean': a, 'std': sigma}, 'Normal Distribution - Method 2')

    # Prompt user for parameters of Method 3 (Uniform / Linear Congruential Generator)
    a = get_user_input("Enter multiplier (a) for Linear Congruential Generator", 5**13)
    c = get_user_input("Enter modulus (c) for Linear Congruential Generator", 2**31)
    data_3 = method_3(a, c)
    mean_3, variance_3 = calculate_mean_and_variance(data_3)
    print(f"Method 3 (Uniform) - Mean: {mean_3}, Variance: {variance_3}")
    chi2_stat_3, p_value_3 = analyze_data(data_3, 'uniform', {}, 'Uniform Distribution - Method 3')

    # Display Chi-square results
    chi2_results = {
        'Method 1 (Exponential)': (chi2_stat_1, p_value_1),
        'Method 2 (Normal)': (chi2_stat_2, p_value_2),
        'Method 3 (Uniform)': (chi2_stat_3, p_value_3)
    }

    chi2_df = pd.DataFrame(chi2_results, index=['Chi-Square', 'p-value'])
    print(chi2_df)
    
if __name__ == "__main__":
    main()