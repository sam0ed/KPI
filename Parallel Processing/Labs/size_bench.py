import pandas as pd
import matplotlib.pyplot as plt

# Load the CSV data into a Pandas DataFrame
df = pd.read_csv('results.csv', names=['MatrixSize', 'TimeTaken', 'Algorithm'])

# Plotting the results
plt.figure(figsize=(10, 5))

# Group the DataFrame by Algorithm to plot each algorithm's performance
for algorithm in df['Algorithm'].unique():
    subset = df[df['Algorithm'] == algorithm]
    plt.plot(subset['MatrixSize'], subset['TimeTaken'], label=algorithm, marker='o')

plt.xlabel('Matrix Size')
plt.ylabel('Time Taken (ms)')
plt.title('Performance of Matrix Multiplication Algorithms')
plt.legend()
plt.grid(True)
plt.show()
