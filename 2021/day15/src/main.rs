use aoc;

fn main() {
    println!("Answer to A: {}", part_a("day15/input.txt"));
    println!("Answer to B: {}", part_b("day15/input.txt"));
}

fn find_paths<'a>(maze: Vec::<Vec::<i64>>) -> Vec::<&'a Vec::<(usize, usize, i64)>> {
    fn find_path<'a>(x: usize, y: usize, maze: &Vec::<Vec::<i64>>, current_path: &'a mut Vec::<(usize, usize, i64)>, all_paths: &'a mut Vec::<&'a Vec::<(usize, usize, i64)>>) {
        current_path.push(
            (x,y,maze[y][x])
        );
        
        if y == maze.len() - 1 && x == maze[0].len() - 1 {
            all_paths.push(current_path);
            return;
        } 

        if x > 0 {
            
        }
    }

    let mut paths = Vec::<&Vec::<(usize, usize, i64)>>::new();
    find_path(0, 0, &maze, &mut Vec::<(usize, usize, i64)>::new(), &mut paths);

    *paths
}

fn part_a(filename: &str) -> i64{
    let maze = aoc::read_grid_into::<i64>(filename);
    let paths = find_paths(maze);
     
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
        assert_eq!(part_a("input_example.txt"), 0);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 0);
    }
}
