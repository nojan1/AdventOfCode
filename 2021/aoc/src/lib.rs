use std::fs;
use std::str::FromStr;
use std::fmt::Debug;

pub fn read_grid(filename: &str) -> Vec<Vec<char>>{
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(|x| x.chars().collect())
        .collect::<Vec<Vec<char>>>()
} 

pub fn read_ints(filename: &str) -> Vec<u32> {
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(|x| x.parse().expect("Not an integer"))
        .collect()
}

pub fn read_lines(filename: &str) -> Vec<String> {
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(String::from)
        .collect()
}

pub fn read_into<T: FromStr>(filename: &str) -> Vec<T> where <T as FromStr>::Err: Debug {
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(|x| str::parse(x).unwrap())
        .collect()
}

// #[cfg(test)]
// mod tests {
//     #[test]
//     fn it_works() {
//         let result = 2 + 2;
//         assert_eq!(result, 4);
//     }
// }
