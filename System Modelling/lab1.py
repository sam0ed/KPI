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

# Function to plot histogram and perform chi-square test
def analyze_data(data, theoretical_dist, params, title):
    # Plot the histogram of the data
    plt.figure(figsize=(8, 6))
    plt.hist(data, bins=20, density=True, alpha=0.6, color='g', label='Generated Data')
    
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

    # Chi-square test
    observed, bins = np.histogram(data, bins=20, density=False)
    # print("Observed ", observed)
    # print("Bins ", bins)
    expected = None
    
    # if theoretical_dist == 'exponential':
    #     expected = stats.expon.pdf(bins[:-1], scale=1/params['lambda'])
    # elif theoretical_dist == 'normal':
    #     expected = stats.norm.pdf(bins[:-1], loc=params['mean'], scale=params['std'])
    # elif theoretical_dist == 'uniform':
    #     expected = np.ones_like(observed) / len(observed)
        
        
    # Compute the bin centers (this gives a more accurate representation than using bin edges)
    bin_centers = (bins[:-1] + bins[1:]) / 2

    # Now calculate the expected frequencies by scaling the PDF
    if theoretical_dist == 'exponential':
        pdf_values = stats.expon.pdf(bin_centers, scale=1/params['lambda'])
        expected = pdf_values * np.diff(bins) * len(data)
    elif theoretical_dist == 'normal':
        pdf_values = stats.norm.pdf(bin_centers, loc=params['mean'], scale=params['std'])
        expected = pdf_values * np.diff(bins) * len(data)
    elif theoretical_dist == 'uniform':
        expected = np.ones_like(observed) * (len(data) / len(observed))

    total_observed = np.sum(observed)
    expected = expected * (total_observed / np.sum(expected))
    
    chi2_stat, p_value = stats.chisquare(observed, expected)
    
    return chi2_stat, p_value

# Generate data for Method 1 and analyze
lambd = 1
data_1 = method_1(lambd)
chi2_stat_1, p_value_1 = analyze_data(data_1, 'exponential', {'lambda': lambd}, 'Exponential Distribution - Method 1')

# Generate data for Method 2 and analyze
a = 0
sigma = 1
data_2 = method_2(a, sigma)
chi2_stat_2, p_value_2 = analyze_data(data_2, 'normal', {'mean': a, 'std': sigma}, 'Normal Distribution - Method 2')

# Generate data for Method 3 and analyze
data_3 = method_3()
chi2_stat_3, p_value_3 = analyze_data(data_3, 'uniform', {}, 'Uniform Distribution - Method 3')

# Display Chi-square results
chi2_results = {
    'Method 1 (Exponential)': (chi2_stat_1, p_value_1),
    'Method 2 (Normal)': (chi2_stat_2, p_value_2),
    'Method 3 (Uniform)': (chi2_stat_3, p_value_3)
}

chi2_df = pd.DataFrame(chi2_results, index=['Chi-Square', 'p-value'])
print(chi2_df)