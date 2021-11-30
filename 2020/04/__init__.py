import re

required_fields = ["byr","iyr","eyr","hgt", "hcl","ecl","pid"]

def parse_passports(data):
    current_passport = []
    passports = [current_passport]

    for line in data.split("\n"):
        if(line == ""):
            current_passport = []
            passports.append(current_passport)
        else:
            for pair in line.split(" "):
                key, value = pair.split(":")
                current_passport.append((key.strip(), value.strip()))

    return passports

def is_valid(passport, validate_values):
    def is_in_passport(field):
        def has_valid_values(value):
            if field == "byr":
                return int(value) >= 1920 and int(value) <= 2002
            elif field == "iyr":
                return int(value) >= 2010 and int(value) <= 2020
            elif field == "eyr":
                return int(value) >= 2020 and int(value) <= 2030
            elif field == "hgt":
                unit = value[-2:]
                amount = value[0:-2]

                if(unit == "cm"):
                    return int(amount) >= 150 and int(amount) <= 193
                else:
                    return int(amount) >= 59 and int(amount) <= 76

            elif field == "hcl":
                return re.match(r"^#[0-9a-f]{6}$", value)
            elif field == "ecl":
                return len(value) == 3 and any(x for x in ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"] if x in value)
            elif field == "pid":
                return re.match(r"^\d{9}$", value)

        return any(x for x in passport if x[0] == field and (not validate_values or has_valid_values(x[1])))

    return all(is_in_passport(x) for x in required_fields)

def part1(data):
    passports = parse_passports(data)
    return len([x for x in passports if is_valid(x, False)])

def part2(data):
    passports = parse_passports(data)
    return len([x for x in passports if is_valid(x, True)])

tests = [
    ("04_test1", 2, part1),
    ("04_test2", 4, part2)
]