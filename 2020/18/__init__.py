def calculate(line):
    def handle(expression):
        buffer = ""
        operand1 = None
        operand2 = None
        last_operator = None
        i = 0
        while i < len(expression):
            c = expression[i]
            if c == "+" or c == "*":
                if len(buffer) > 0:
                    number = int(buffer)
                    buffer = ""

                    if operand1 == None:
                        operand1 = number
                    else:
                        operand2 = number

                if last_operator != None:
                    if last_operator == "+":
                        operand1 += operand2
                    elif last_operator == "*":
                        operand1 *= operand2

                    operand2 = None

                last_operator = c
            elif c == "(":
                buffer = ""
                
                if operand1 == None:
                    operand1, end_index = handle(expression[i+1:])
                else:
                    operand2, end_index = handle(expression[i+1:])

                i += end_index + 1
            elif c == ")":
                if len(buffer) > 0:
                    operand2 = int(buffer)
                    buffer = ""

                if last_operator != None:
                    if last_operator == "+":
                        operand1 += operand2
                    elif last_operator == "*":
                        operand1 *= operand2

                return (operand1, i)
            else:
                buffer += c

            i += 1

        if len(buffer) > 0:
            operand2 = int(buffer)

        if last_operator != None:
            if last_operator == "+":
                operand1 += operand2
            elif last_operator == "*":
                operand1 *= operand2

        return (operand1, i)

    return(handle(line.replace(" ", ""))[0])


def part1(data):
    answers = list(map(calculate, data.split("\n")))
    return sum(answers)


def part2(data):
    return None


tests = [
    ("18_test3", 13632, part1),
    ("18_test2", 26, part1),
    ("18_test1", 26335, part1),
    # ("<input file name minus txt>", <expected result>, part1),
]
