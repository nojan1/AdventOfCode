use aoc;
use std::collections::HashMap;

fn main() {
    println!("Answer to A: {}", part_a("day12/input.txt"));
    println!("Answer to B: {}", part_b("day12/input.txt"));
}

struct Room<'a>{
    name: String,
    paths: Vec::<&'a Box<Room<'a>>>
}

impl Room<'_> {
    fn new(name: &str) -> Room {
        Room {
            name: String::from(name),
            paths: Vec::<&Box<Room>>::new()
        }
    }
}

fn parse_input(filename: &str) -> HashMap::<&str, Box<Room>> {
    let lines = aoc::read_lines(filename);
    let mut rooms = HashMap::<&str, Box<Room>>::new();

    for line in lines {
        let parts = line.split("-").collect::<Vec<&str>>();
        let to = rooms.entry(parts[1]).or_insert(Box::new(Room::new(parts[1])));
        rooms.entry(parts[0]).or_insert(Box::new(Room::new(parts[0]))).paths.push(to);
    }

    rooms
}

fn part_a(filename: &str) -> i64{
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
        assert_eq!(part_a("input_example.txt"), 10);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 0);
    }
}
