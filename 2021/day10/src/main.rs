use aoc;
fn main() {
    println!("Answer to A: {}", part_a("day10/input.txt"));
    println!("Answer to B: {}", part_b("day10/input.txt"));
}

fn is_opening_character(c: char) -> bool {
    c == '(' || c == '[' || c == '{' || c == '<'
}

fn is_closing_character(c: char) -> bool {
    c == ')' || c == ']' || c == '}' || c == '>'
}

fn get_closing_character(open: char) -> char {
    match open {
        '(' => ')',
        '[' => ']',
        '{' => '}',
        '<' => '>',
        _ => panic!("What the heck is this character!")
    }
}

fn part_a(filename: &str) -> i64{
    let input = aoc::read_grid(filename);
    let mut score: i64 = 0;

    for line in input {
        let mut seen_openings = Vec::<char>::new();

        for c in line {
            if is_opening_character(c) {
                    seen_openings.push(c);
            }else if get_closing_character(*seen_openings.last().unwrap()) != c{
                score += match c{
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137,
                    _ => 0
                };

                break;
            }else{
                //Must have been a closing character... pop off one
                seen_openings.pop();
            }
        }
    }

    score
}

fn part_b(filename: &str) -> i64 {
    let input = aoc::read_grid(filename);
    let mut scores = Vec::<i64>::new();

    for line in input {
        let mut seen_openings = Vec::<char>::new();
        let mut is_corrupt = false;

        for c in line {
            if is_opening_character(c) {
                seen_openings.push(c);
            }else if get_closing_character(*seen_openings.last().unwrap()) != c{
                is_corrupt = true;
                break;
            }else{
                seen_openings.pop();
            }
        }

        if !is_corrupt {
            seen_openings.reverse();
            let autocomplete_score = seen_openings.iter()
                .map(|c| get_closing_character(*c))
                .map(|c| match c {
                    ')' => 1,
                    ']' => 2,
                    '}' => 3,
                    '>' => 4,
                    _ => 0
                })
                .fold(0, |a,b| (a * 5) + b);

            scores.push(autocomplete_score);
        }
    }

    println!("{:?}", scores);
    scores[scores.len() / 2]
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 26397);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 288957);
    }
}
