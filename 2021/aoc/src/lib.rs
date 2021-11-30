pub fn read_input(filename: &str) -> Vec<Vec<char>>{
    fs::read_to_string(filename).unwrap()
        .lines().collect::<Vec<&str>>().iter()
        .map(|x| x.chars().collect()).collect::<Vec<Vec<char>>>()
} 

// #[cfg(test)]
// mod tests {
//     #[test]
//     fn it_works() {
//         let result = 2 + 2;
//         assert_eq!(result, 4);
//     }
// }
