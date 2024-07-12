from time import time
import numpy as np
import matplotlib.pyplot as plt
import multiprocessing


def calculate_r2(Y_actual, Y_predicted):
    diff = Y_actual - Y_predicted
    mse = np.mean(diff**2)
    variance_Y = np.var(Y_actual)
    r2 = 1 - (mse / variance_Y)
    return r2


def rastrigin(X):
    A = 10
    return A * 2 + (X[:, 0]**2 - A * np.cos(2 * np.pi * X[:, 0])) + (X[:, 1]**2 - A * np.cos(2 * np.pi * X[:, 1]))


def generate_data(n_samples, n_start, n_end):
    x = np.linspace(n_start, n_end, n_samples)
    y = np.linspace(n_start, n_end, n_samples)
    x, y = np.meshgrid(x, y)
    X_grid = np.vstack([x.ravel(), y.ravel()]).T

    z = rastrigin(X_grid).reshape(-1, 1)
    return X_grid, z


def sigmoid(z):
    return 1 / (1 + np.exp(-z))


def sigmoid_derivative(z):
    return z * (1 - z)


def mse_loss(y_true, y_pred):
    return np.mean((y_true - y_pred) ** 2)


def forward(X, W1, b1, W2, b2, W3, b3):
    z1 = np.dot(X, W1) + b1
    a1 = sigmoid(z1)
    z2 = np.dot(a1, W2) + b2
    a2 = sigmoid(z2)
    z3 = np.dot(a2, W3) + b3
    return z3, a1, a2


def backward(X, y, output, a1, a2, W1, W2, W3):
    m = y.shape[0]

    d_z3 = output - y
    d_W3 = (1/m) * np.dot(a2.T, d_z3)
    d_b3 = (1/m) * np.sum(d_z3, axis=0, keepdims=True)

    d_a2 = np.dot(d_z3, W3.T)
    d_z2 = d_a2 * sigmoid_derivative(a2)
    d_W2 = (1/m) * np.dot(a1.T, d_z2)
    d_b2 = (1/m) * np.sum(d_z2, axis=0, keepdims=True)

    d_a1 = np.dot(d_z2, W2.T)
    d_z1 = d_a1 * sigmoid_derivative(a1)
    d_W1 = (1/m) * np.dot(X.T, d_z1)
    d_b1 = (1/m) * np.sum(d_z1, axis=0, keepdims=True)

    return d_W1, d_b1, d_W2, d_b2, d_W3, d_b3


def update_parameters(W1, b1, W2, b2, W3, b3, gradients, learning_rate):
    d_W1, d_b1, d_W2, d_b2, d_W3, d_b3 = gradients
    # Update weights and biases
    W1 -= learning_rate * d_W1
    b1 -= learning_rate * d_b1
    W2 -= learning_rate * d_W2
    b2 -= learning_rate * d_b2
    W3 -= learning_rate * d_W3
    b3 -= learning_rate * d_b3
    return W1, b1, W2, b2, W3, b3


def process_batch(batch, W1, b1, W2, b2, W3, b3):
    try:
        X_batch, y_batch = batch
        output, a1, a2 = forward(X_batch, W1, b1, W2, b2, W3, b3)
        gradients = backward(X_batch, y_batch, output, a1, a2, W1, W2, W3)
        return gradients
    except Exception as e:
        print(f"Error processing batch: {e}")
        raise


def parallel_fit_batch(X, y, epochs, batch_size, W1, b1, W2, b2, W3, b3, learning_rate, batch_group_size):
    pool = multiprocessing.Pool()
    history = []
    time_array = []
    for epoch in range(epochs):
        total_time = 0
        startTime = time()
        permutation = np.random.permutation(X.shape[0])
        X_shuffled = X[permutation]
        y_shuffled = y[permutation]

        batches = [(X_shuffled[i:i + batch_size], y_shuffled[i:i + batch_size])
                   for i in range(0, X.shape[0], batch_size)]

        batch_groups = [batches[i:i + batch_group_size]
                        for i in range(0, len(batches), batch_group_size)]

        for batch_group in batch_groups:

            gradients = pool.starmap(
                process_batch, [(batch, W1, b1, W2, b2, W3, b3) for batch in batch_group])

            avg_gradients = [np.sum(
                [grad[i] for grad in gradients], axis=0) / batch_group_size for i in range(6)]
            W1, b1, W2, b2, W3, b3 = update_parameters(
                W1, b1, W2, b2, W3, b3, avg_gradients, learning_rate)

        total_time = time() - startTime
        time_array.append(total_time)

        output, _, _ = forward(X_shuffled, W1, b1, W2, b2, W3, b3)
        loss = mse_loss(y_shuffled, output)
        history.append(loss)
        print(f"Epoch {epoch+1}/{epochs}, Loss: {loss}, Time: {total_time}")

    pool.close()
    pool.join()
    return W1, b1, W2, b2, W3, b3, time_array, history


def fit_batch(X, y, epochs, batch_size, W1, b1, W2, b2, W3, b3, learning_rate, batch_group_size):
    time_array = []
    history = []
    for epoch in range(epochs):
        total_time = 0
        startTime = time()
        permutation = np.random.permutation(X.shape[0])
        X_shuffled = X[permutation]
        y_shuffled = y[permutation]

        batches = [(X_shuffled[i:i + batch_size], y_shuffled[i:i + batch_size])
                   for i in range(0, X.shape[0], batch_size)]

        batch_groups = [batches[i:i + batch_group_size]
                        for i in range(0, len(batches), batch_group_size)]

        for i, batch_group in enumerate(batch_groups):
            group_gradients = []

            for batch in batch_group:
                gradients = process_batch(batch, W1, b1, W2, b2, W3, b3)
                group_gradients.append(gradients)

            avg_gradients = [np.sum(
                [grad[i] for grad in group_gradients], axis=0) / batch_group_size for i in range(6)]
            W1, b1, W2, b2, W3, b3 = update_parameters(
                W1, b1, W2, b2, W3, b3, avg_gradients, learning_rate)

        total_time = time() - startTime
        time_array.append(total_time)
        output, _, _ = forward(X_shuffled, W1, b1, W2, b2, W3, b3)
        loss = mse_loss(y_shuffled, output)
        history.append(loss)
        print(f"Epoch {epoch+1}/{epochs}, Loss: {loss}, Time: {total_time}")
        print()
    return W1, b1, W2, b2, W3, b3, time_array, history


def main():
    np.random.seed(42)

    input_size = 2
    hidden_size = 128
    output_size = 1
    W1 = np.random.randn(input_size, hidden_size)
    b1 = np.zeros((1, hidden_size))
    W2 = np.random.randn(hidden_size, hidden_size)
    b2 = np.zeros((1, hidden_size))
    W3 = np.random.randn(hidden_size, output_size)
    b3 = np.zeros((1, output_size))
    learning_rate = 0.05
    batch_group_size = 8  # multiprocessing.cpu_count()

    input_range = 5.12
    input_density = 400

    X_train, Y_train = generate_data(input_density, -input_range, input_range)
    X_test = np.random.uniform(-input_range, input_range, (5000, 2))
    Y_test = rastrigin(X_test)

    W1, b1, W2, b2, W3, b3, time_array, history = parallel_fit_batch(
        X_train, Y_train, epochs=300, batch_size=256, W1=W1, b1=b1, W2=W2, b2=b2, W3=W3, b3=b3, learning_rate=learning_rate, batch_group_size=batch_group_size)
    # W1, b1, W2, b2, W3, b3, time_array, history = fit_batch(X_train, Y_train, epochs=300, batch_size=256, W1=W1, b1=b1, W2=W2, b2=b2,
    #         W3=W3, b3=b3, learning_rate=learning_rate, batch_group_size=batch_group_size)

    print("Epoch time", sum(time_array)/len(time_array))

    Y_pred, _, _ = forward(X_test, W1, b1, W2, b2, W3, b3)
    r2 = calculate_r2(Y_test, Y_pred.flatten())
    print(f"R^2 score: {r2:.4f}")

    plt.figure(figsize=(10, 5))
    plt.plot(history, marker='o', linestyle='-', color='b')
    plt.title('Loss Over Epochs')
    plt.xlabel('Epoch')
    plt.ylabel('Loss')
    plt.grid(True)
    plt.show()

    plt.scatter(Y_test, Y_pred)
    plt.xlabel('Actual Values')
    plt.ylabel('Predicted Values')
    plt.title('Actual vs. Predicted Rastrigin Function Values')
    plt.show()


if __name__ == '__main__':
    main()
