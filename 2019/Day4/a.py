RANGE_FROM = 248345
RANGE_TO = 746315

def generate(low, high):
    for i in range(low, high+1):
        yield str(i)

def isValid(password):
    if len(password) != 6:
        return False

    hasDoubles = False
    for i, digit in enumerate(password):
        if i > 0:
            lastDigit = password[i-1]

            if digit < lastDigit:
                return False
            elif digit == lastDigit:
                hasDoubles = True

    return hasDoubles

validPasswords = list(filter(isValid, generate(RANGE_FROM, RANGE_TO)))
print('A: %i' % len(validPasswords))