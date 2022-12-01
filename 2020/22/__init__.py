from collections import deque
from functools import reduce

def parse_decks(data):
    decks = []
    current_deck = None

    for line in data.split("\n"):
        if line.startswith("Player"):
            current_deck = deque()
            decks.append((line[0:-1], current_deck))
        elif line != "":
            current_deck.append(int(line))

    return decks

def play_game(decks, is_recursive):
    deck_history = [[] for _ in decks]
    round_num = 1
    while True:
        players_with_cards_left = [x for x in decks if len(x[1]) > 0]
        if(len(players_with_cards_left) == 1):
            return round_num, players_with_cards_left[0]

        if is_recursive:
            for i, deck in enumerate(decks):
                current_deck_list = list(deck[1])
                if current_deck_list in deck_history[i]:
                    return round_num, players_with_cards_left[0]
                else:
                    deck_history[i].append(current_deck_list)

        played_cards = list(map(lambda x: (x[1].popleft(), x[1], x[0]), players_with_cards_left))
        
        if is_recursive and all(x for x in played_cards if len(x[1]) >= x[0]):
            new_decks = list(map(lambda x: (x[2], deque( list(x[1][0:x[0]]) ) ), played_cards))
            print(new_decks)
            continue

        all_cards = list(map(lambda x: x[0], played_cards))
        high_card = reduce(max, all_cards)

        for _,deck,player in filter(lambda x: x[0] == high_card, played_cards):
            all_cards.sort(reverse=True)
            for card in all_cards:
                deck.append(card)

        round_num += 1
        
def part1(data):
    decks = parse_decks(data)
    _, winner = play_game(decks, False)
    score = reduce(lambda acc, cur: acc + ((cur[0] + 1) * cur[1]), (x for x in enumerate(list(winner[1])[::-1])), 0)

    return score

def part2(data):
    decks = parse_decks(data)
    _, winner = play_game(decks, True)
    score = reduce(lambda acc, cur: acc + ((cur[0] + 1) * cur[1]), (x for x in enumerate(list(winner[1])[::-1])), 0)

    return score

tests = [
    ("22_test1", 306, part1),
    ("22_test2", 291, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
