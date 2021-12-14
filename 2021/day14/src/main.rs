use aoc;
use std::collections::HashMap;

fn main() {
    println!("Answer to A: {}", part_a("day14/input.txt"));
    println!("Answer to B: {}", part_b("day14/input.txt"));
}

struct PolymerChain<'a> {
    value: char,
    prev: Option<&'a Box<PolymerChain<'a>>>,
    next: Option<&'a Box<PolymerChain<'a>>>
}

impl PolymerChain<'_> {
    fn new<'a>(input: String) -> &'a Box<PolymerChain<'a>> {
        let mut input = input.chars().rev().collect::<Vec::<char>>();

        let first = &mut Box::new(PolymerChain { 
            value: input.pop().unwrap(),
            prev: None,
            next: None
        });

        let mut current = &first;

        while let Some(cur_char) = input.pop() {
            current.next = Some(&Box::new(PolymerChain {
                value: cur_char,
                prev: Some(current),
                next: None
            }));

            current = &mut current.next.unwrap();
        }

        first
    }

    fn match_and_insert(&self, needle: String, reduction: String) {

    }
}

fn parse_input<'a>(filename: &str) -> (&'a Box<PolymerChain>, HashMap::<String, String>) {
    let mut polymer_reductions = HashMap::<String, String>::new();
    let mut lines = aoc::read_lines(filename).into_iter();

    let start_value: String = lines.next().unwrap();
    lines.next();

    while let Some(line) = lines.next() {
        let parts = line.split(" -> ").collect::<Vec::<&str>>();
        polymer_reductions.insert(String::from(parts[0]), String::from(parts[1]));
    }

    (PolymerChain::new(start_value), polymer_reductions)
}

fn part_a(filename: &str) -> i64{
    let (polymer_chain, polymer_reductions) = parse_input(filename);

    0
}

fn part_b(filename: &str) -> i64 {
    0
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 1588);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 0);
    }
}
