import os
import math


def factorial(value: int):
    fact = 1
    if value == 0:
	    return 1
    elif value > 0:
        for i in range(1, value+1):
            fact *= i
        return fact
    elif value < 0:
        print(" Can`t get factorial of this number.\n")
        exit(-1)

def function(x: float):
    numerator=float(0)
    denominator=float(0)
	
    for k in range(0,5):
        numerator += pow(x, (2 * k) + 1)/factorial(2*k+1)
        denominator += pow(x, 3 * k) / factorial(3 * k)
    return (numerator / denominator)

def main():
    y=float(input("Enter the y value please: "))
    result = float((1.7 * function(0.25) + 2 * function(1 + y)) / (6 - function(pow(y, 2) - 1)))
    print("The result of the equation is ",result)

main()
    
os.system("pause")