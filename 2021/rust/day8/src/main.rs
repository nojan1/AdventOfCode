use aoc;
use std::str::FromStr;
use std::collections::HashMap;

fn main() {
    println!("Answer to A: {}", part_a("day8/input.txt"));
    println!("Answer to B: {}", part_b("day8/input.txt"));
}

struct Entry {
    inputs: Vec::<Vec::<char>>,
    outputs: Vec::<Vec::<char>>,
    translations: HashMap::<char, char>
}

impl Entry {
    fn is_fully_decoded(&self) -> bool {
        self.translations.iter().all(|(key,value)| *value != ' ')
    }
}

impl FromStr for Entry {
    type Err = ();

    fn from_str(s: &str) -> Result<Entry, ()> {
        fn to_char_groups(group_str: &str) -> Vec::<Vec::<char>> {
            group_str.split(" ")
                .map(|s| s.chars().collect())
                .collect()
        }

        let halfs: Vec<&str> = s.split("|").collect();

        Ok(Entry {
            inputs: to_char_groups(halfs[0]),
            outputs: to_char_groups(halfs[1]),
            translations: HashMap::from([
                ('a', ' '),
                ('b', ' '),
                ('c', ' '),
                ('d', ' '),
                ('e', ' '),
                ('f', ' '),
                ('g', ' ')
            ])
        })
    }
}

fn part_a(filename: &str) -> u32{
    let entries = aoc::read_into::<Entry>(filename);

    let mut count = 0;
    for entry in entries {
        for output in &entry.outputs {
            let output_len = output.len();

            if output_len == 2 || output_len == 4 || output_len == 3 || output_len == 7 {
                count += 1;
            }
        }
    }

    count
}

fn part_b(filename: &str) -> i64 {
    let mut entries = aoc::read_into::<Entry>(filename);

    for entry in &mut entries {
        for input in &entry.inputs {
            let input_len = input.len();

            match input_len {
                2 => {
                    // This is a 1
                    entry.translations.entry('c').and_modify(|x| *x = input[0]);
                    entry.translations.entry('f').and_modify(|x| *x = input[1]);
                },
                3 => {
                    entry.translations.entry('a').and_modify(|x| *x = input[0]);
                    entry.translations.entry('c').and_modify(|x| *x = input[1]);
                    entry.translations.entry('f').and_modify(|x| *x = input[2]);
                },
                4 => {
                    entry.translations.entry('a').and_modify(|x| *x = input[0]);
                    entry.translations.entry('c').and_modify(|x| *x = input[1]);
                    entry.translations.entry('d').and_modify(|x| *x = input[2]);
                    entry.translations.entry('f').and_modify(|x| *x = input[3]);
                },
                _ => {}
            }
        }

        println!("{:?}", entry.translations);

        // while !&entry.is_fully_decoded() {
            
        // }
    }

    0
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 26);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 61229);
    }
}
