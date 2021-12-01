use std::fs;

pub fn read_grid(filename: &str) -> Vec<Vec<char>>{
    fs::read_to_string(filename).unwrap()
        .lines().collect::<Vec<&str>>().iter()
        .map(|x| x.chars().collect()).collect::<Vec<Vec<char>>>()
} 

pub fn read_ints(filename: &str) -> Vec<u32> {
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(|x| x.parse().expect("Not an integer"))
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
