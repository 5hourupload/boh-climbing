import sys


def recursion(string):
    if len(string) <= 2:
        return string
    else:
        first = string[0]
        index = 1
        output = ""
        while index < len(string):
            comp = first + string[index]
            output = output + " " + comp

        temp = string[1:]
        output = output + " " + recursion(temp)
        return output


for line in sys.stdin:
    string = line.rstrip().split(" ")
    recursion(string)
