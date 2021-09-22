import os
import math

number = int(input("Enter the number you want to get the digit root from:"))
if number < 0:
    number *= (-1)

while number >= 10:
    copy = number
    counter = 0
    sum = 0

    while abs(copy) >= 1:
        copy /= 10
        counter += 1

    i = int(pow(10, counter - 1))

    while i >= 1:
        add = int(number / i)
        sum += add
        number -= (add * i)
        i /= 10

    number = sum

print("The digit root of the entered value is: ", number)
os.system("pause")
