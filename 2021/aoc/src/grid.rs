use std::str::FromStr;
use std::fmt::Debug;

use crate::read_grid_into;

pub struct Grid<T: FromStr> {
    pub width: usize,
    pub height: usize,
    items: Vec::<Vec::<GridItem<T>>>
}

pub struct GridItem<T> {
    x: usize,
    y: usize,
    value: T
}

impl<T: FromStr> Grid<T> where <T as FromStr>::Err: Debug {
    pub fn new(filename: &str) {
        let data = read_grid_into::<T>(filename);

    }

    pub fn get_neighbours(&self, x: usize, y: usize) -> Option<&[GridItem<T>]>{
        None
    }
}
