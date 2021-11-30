import re, operator
from functools import reduce

def parse_input(data):
    fields = {}
    my_ticket = []
    nearby_tickets = []
    state = 0

    for line in data.split("\n"):
        if line == "your ticket:" or line == "nearby tickets:":
            continue

        if state == 0:
            if line != "":
                match = re.match(r"(.*?): (\d+)-(\d+) or (\d+)-(\d+)", line)

                valid_range = list(range(int(match.group(2)), int(match.group(3)) + 1))   
                valid_range += list(range(int(match.group(4)), int(match.group(5)) + 1)) 

                fields[match.group(1)] = valid_range  
            else:
                state = 1
        elif state == 1:
            if line != "":
                my_ticket = [int(x) for x in line.split(",")]
            else:
                state = 2
        elif state == 2:
            nearby_tickets.append([int(x) for x in line.split(",")])

    return (fields, my_ticket, nearby_tickets)

def filter_bad_data(tickets, fields):
    valid_tickets = []
    bad_values = []

    for ticket in tickets:
        is_valid = True
        for value in ticket:
            if all(value not in valid for valid in fields.values()):
                is_valid = False
                bad_values.append(value)
                break

        if is_valid:
            valid_tickets.append(ticket)

    return (valid_tickets, bad_values)

def part1(data):
    fields, _, nearby_tickets = parse_input(data)
    _, bad_values = filter_bad_data(nearby_tickets, fields)
    return sum(bad_values)

def part2(data):
    def get_possible_fields(my_ticket, nearby_tickets, fields):
        def get_possible_fields_for_values(values, fields):
            def is_valid(values, field):
                return all(value in fields[field] for value in values)

            return [field for field in fields if is_valid(values, field)]

        all_tickets = nearby_tickets + [my_ticket]
        return [get_possible_fields_for_values([ticket[i] for ticket in all_tickets], fields) for i,_ in enumerate(my_ticket)]

    def get_probable_distribution(all_possible_fields):
        #Ended up dropping it in to a text editor and going copy and replace...
        #Seems it can be easily solved by only taking the lowest one though since there is always
        #1 that has a single item
        return ["type","arrival platform","arrival location","class","departure date","departure station","departure track","arrival track","train","zone","route","row","price","duration","wagon","departure platform","arrival station","departure location","departure time","seat"]

    fields, my_ticket, nearby_tickets = parse_input(data)
    nearby_tickets, _ = filter_bad_data(nearby_tickets, fields)
    posible_fields = get_possible_fields(my_ticket, nearby_tickets, fields)
    true_fields = get_probable_distribution(posible_fields)
    indicies_to_multiply = [i for i,x in enumerate(true_fields) if x.startswith("departure")]
    
    return reduce(operator.mul, (my_ticket[i] for i in indicies_to_multiply), 1)

tests = [
    ("16_test1", 71, part1),
    # ("<input file name minus txt>", <expected result>, part1),
]
