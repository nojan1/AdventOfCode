
use std::collections::HashMap;
use std::str::FromStr;
use std::fmt::Debug;

use crate::read_grid_into;

pub struct Grid<'a, T: FromStr> {
    pub width: usize,
    pub height: usize,
    items: HashMap::<(usize, usize),GridItem<'a, T>>
}

pub struct GridItem<'a, T> {
    pub x: usize,
    pub y: usize,
    pub value: &'a T
}

impl<T: FromStr> Grid<'_, T> where <T as FromStr>::Err: Debug {
    pub fn new(filename: &str) {
        let data = read_grid_into::<T>(filename);

        let mut items = HashMap::<(usize, usize), GridItem<T>>::new();
        for (y,row) in data.iter().enumerate() {
            for (x, value) in row.iter().enumerate() {
                items.insert((x,y), GridItem { x, y, value });
            }
        }
    }

    pub fn get(&self, x: usize, y: usize) -> Option<&GridItem<T>> {
        self.items.get(&(x,y))
    }

    pub fn get_neighbours(&self, x: usize, y: usize, include_diagonal: bool) -> Vec::<&GridItem<T>>{
        let mut neighbours = Vec::<&GridItem<T>>::new();
        let x = x as i32;
        let y = y as i32;

        for (xdiff, ydiff) in [(0,1), (1,1), (1,0), (1,-1), (0,-1),(-1,-1),(-1,0),(-1,1)] {
            if !include_diagonal && xdiff != 0 && ydiff != 0 {
                continue;
            }

            if (xdiff < 0 && x == 0) || (ydiff < 0 && y == 0) || x + xdiff > (self.width as i32) - 1 || y + ydiff > (self.height as i32) - 1 {
                continue;
            }

            let lookup_x = (x + xdiff) as usize;
            let lookup_y = (y + ydiff) as usize;

            if let Some(item) = self.get(lookup_x, lookup_y) {
                neighbours.push(item);
            }
        }

        neighbours
    }

    pub fn iter_mut(&self) -> std::collections::hash_map::ValuesMut<(usize,usize), GridItem<T>>
    {
        self.items.values_mut()
    }
}
