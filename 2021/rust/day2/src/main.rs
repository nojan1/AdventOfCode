use std::str::FromStr;
use aoc;

enum Instruction {
    Forward(i32),
    Up(i32),
    Down(i32)
}

impl FromStr for Instruction {
    type Err = ();

    fn from_str(s: &str) -> Result<Instruction, ()> {
        let parts = s.split(" ").collect::<Vec<&str>>();
        let amount = parts[1].parse::<i32>().unwrap();

        match parts[0] {
            "forward" => Ok(Instruction::Forward(amount)),
            "down" => Ok(Instruction::Down(amount)),
            "up" => Ok(Instruction::Up(amount)),
            _ => Err(())
        }
    }
}

fn main() {
    println!("Answer to A: {}", part_a("day2/input.txt"));
    println!("Answer to B: {}", part_b("day2/input.txt"));
}

fn part_a(filename: &str) -> i32{
    let instructions = aoc::read_into::<Instruction>(filename);
    let mut location = (0,0);

    for instruction in instructions {
        match instruction {
            Instruction::Forward(amount) => {
                location.0 += amount
            },
            Instruction::Up(amount) => {
                location.1 -= amount;
            },
            Instruction::Down(amount) => 
                location.1 += amount
        }
    }

    location.0 * location.1
}

fn part_b(filename: &str) -> i32 {
    let instructions = aoc::read_into::<Instruction>(filename);
    let mut location: (i32, i32) = (0,0);
    let mut aim = 0;

    for instruction in instructions {
        match instruction {
            Instruction::Forward(amount) => {
                location.0 += amount;
                location.1 += aim * amount; 
            },
            Instruction::Up(amount) => {
                aim -= amount;
            },
            Instruction::Down(amount) => {
                aim += amount
            }
        }
    }

    location.0 * location.1
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 150);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 900);
    }
}
