#include <iostream>
#include <fstream>
#include <string>
#include <unordered_map>
#include <vector>

using namespace std;

vector<string> allVisited;

int follow_path(int step, string current, unordered_map<string, vector<string>> &caves, string visited, bool visitedTwice) {
  if(current == "end") {
    if(find(allVisited.begin(), allVisited.end(), visited) != allVisited.end()) return 0;

    allVisited.push_back(visited);
    //cout << visited << "end" << endl;

    return 1;
  }

  int count = 0;
    
  visited += current;

  for (const auto& destination: caves[current]) {
    if(destination == "start") continue;
    
    if(islower(destination[0])) {
      int count = std::count(visited.begin(), visited.end(), destination[0]);

      if(step == 2 && count == 1 && !visitedTwice)
        visitedTwice = true;
      else if(count > 0)
        continue;
    }

    if(destination == "start" || (islower(destination[0]) && std::count(visited.begin(), visited.end(), destination[0]) >= step)) continue;
    //cout << current << "->" << destination << endl;
    count += follow_path(step, destination, caves, visited, visitedTwice);
  }
  
  return count;
}

int count_paths(unordered_map<string, vector<string>> &caves, int step) {
  int count = 0;

  for( const auto& n : caves["start"] ) {
      count += follow_path(step, n, caves, "", false);
  }

  return count;
}


int main() {
  unordered_map<string, vector<string>> caves = {};
  string line;
  ifstream myfile ("2021/cpp/day12/example1.txt");
  if (myfile.is_open())
  {
    while ( getline (myfile,line) )
    {
      int pos = line.find("-");
      string from = line.substr(0, pos);
      string to = line.substr(pos + 1);

      if(caves.find(to) == caves.end()){
        caves[to] = { from };
      }else {
        caves[to].push_back(from);
      }
      
      if(caves.find(from) == caves.end()){
        caves[from] = { to };
      }else {
        caves[from].push_back(to);
      }

      //cout << from << "-" << to << endl;
    }
    myfile.close();

    int num_paths = count_paths(caves, 1);
    cout << "A: There are " << num_paths << " paths" << endl;

    allVisited.clear();
    num_paths = count_paths(caves, 2);
    cout << "B: There are " << num_paths << " paths" << endl;
  }

  else cout << "Unable to open file"; 

  return 0;
} 
