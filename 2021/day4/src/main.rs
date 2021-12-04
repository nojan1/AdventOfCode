use aoc;
use std::iter::Iterator;

struct Board {
    slots: Vec::<Vec::<u32>>,
    checked: [[bool; 5]; 5],
    has_bingo: bool,
    score: u32
}

impl Board {
    fn parse(mut lines: impl Iterator<Item = String>) -> Option<Board> {
        let checked = [[false; 5]; 5];
        let mut slots = Vec::<Vec::<u32>>::new();

        for _ in 0..5 {
            let number_line: Vec<u32> = lines.next()?
                .split(" ")
                .filter(|x| x != &"")
                .map(|x| x.trim().parse().unwrap())
                .collect();

            slots.push(number_line);
        }

        lines.next(); //Read empty line and ignore result

        Some(Board {slots, checked, has_bingo: false, score: 0})
    }

    fn mark_number(&mut self, number: u32) {
        for x in 0..5 {
            for y in 0..5 {
                if self.slots[x][y] == number {
                    self.checked[x][y] = true;

                    if !self.has_bingo {
                        self.has_bingo = 
                            (self.checked[0][y] && self.checked[1][y] && self.checked[2][y] && self.checked[3][y] && self.checked[4][y]) ||
                            (self.checked[x][0] && self.checked[x][1] && self.checked[x][2] && self.checked[x][3] && self.checked[x][4]);

                        if self.has_bingo {
                            //Calculate score
                            let mut non_checked_score = 0;
                            for x2 in 0..5 {
                                for y2 in 0..5 {
                                    if !self.checked[x2][y2] {
                                        non_checked_score += self.slots[x2][y2];
                                    }
                                }
                            }

                            self.score = non_checked_score * number;
                        }
                    }
                    
                    return
                }
            }
        }
    }
}

fn main() {
    println!("Answer to A: {}", part_a("day4/input.txt"));
    println!("Answer to B: {}", part_b("day4/input.txt"));
}

fn parse_input(filename: &str) -> (Vec::<u32>, Vec::<Board>) {
    let mut lines = aoc::read_lines(filename).into_iter();
    let numbers: Vec<u32> = lines.next().unwrap()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    lines.next(); //Ignore

    let mut boards = Vec::<Board>::new();
    while let Some(board) = Board::parse(&mut lines) {
        boards.push(board);
    }

    (numbers, boards)
}

fn part_a(filename: &str) -> u32{
    let (numbers, mut boards) = parse_input(filename);

    for number in numbers {
        for board in &mut boards {
            board.mark_number(number);
        }

        let boards_with_bingo: Vec<&Board> = boards.iter().filter(|x| x.has_bingo).collect();
        if boards_with_bingo.len() > 0 {
            //We have bingo!
            return boards_with_bingo.iter().map(|x| x.score).max().unwrap();
        }
    }

    0
}

fn part_b(filename: &str) -> u32 {
    let (numbers, mut boards) = parse_input(filename);

    let mut boards_without_bingo = boards.len();
    for number in numbers {
        for board in &mut boards {
            if board.has_bingo {
                continue;
            }

            board.mark_number(number);
            if board.has_bingo {
                boards_without_bingo -= 1;        
                if boards_without_bingo == 0 {
                    return board.score;
                }
            }
        }
    }

    0
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 4512);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 1924);
    }
}
