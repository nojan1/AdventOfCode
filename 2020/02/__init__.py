def parseRow(row):
    fields = row.split(" ")
    limits = fields[0].split("-")

    return (int(limits[0]), int(limits[1]), fields[1][0:-1], fields[2])

def part1(data):
    def isPasswordValid(row):
        (limitLow, limitHigh, letter, password) = row
        correct_chars = [c for c in password if c == letter]
        return len(correct_chars) >= limitLow and len(correct_chars) <= limitHigh

    parsed_data = [parseRow(x) for x in data.split("\n")]
    valid_passwords = [x for x in parsed_data if isPasswordValid(x)] 
    return len(valid_passwords)

def part2(data):
    def isPasswordValid(row):
        (pos1, pos2, letter, password) = row

        is_in_pos1 = password[pos1 - 1] == letter
        is_in_pos2 = password[pos2 - 1] == letter

        return (is_in_pos1 or is_in_pos2) and not is_in_pos1 == is_in_pos2

    parsed_data = [parseRow(x) for x in data.split("\n")]
    valid_passwords = [x for x in parsed_data if isPasswordValid(x)] 
    return len(valid_passwords)

tests = [
    ("02_test1", 2, part1),
    ("02_test1", 1, part2)
]
