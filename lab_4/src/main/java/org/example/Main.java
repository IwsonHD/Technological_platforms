package org.example;

import org.hibernate.engine.jdbc.spi.SchemaNameResolver;

import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        EntityManager entityManager = new EntityManager();
        Scanner scanner = new Scanner(System.in);
        Boolean run = true;
        String name, towerName;
        int level, height;
        while(run){
          switch (scanner.nextLine()){
              case "-q":
              case "--quit":
                  run = false;
                  break;
              case "-am":
              case "--add-mage":
                  System.out.println("Input name ,level and tower's name");
                  name = scanner.next();
                  level = Integer.parseInt(scanner.next());
                  towerName = scanner.next();
                  entityManager.addNewMage(name, level, towerName);
                  break;
              case "-at":
              case "--add-tower":
                  System.out.println("Input tower's name and height");
                  name = scanner.next();
                  height = Integer.parseInt(scanner.next());
                  entityManager.addNewTower(name, height);
                  break;
              case "-dt":
              case "--delete-tower":
                  System.out.println("Input tower's name");
                  towerName = scanner.next();
                  entityManager.removeTower(towerName);
                  break;
              case "-dm":
              case "--delete-mage":
                  System.out.println("Input mage's name");
                  name = scanner.next();
                  entityManager.removeMage(name);
                  break;
              case "-s":
              case "--show-all":
                  entityManager.showMagesByTowers();
                  break;
              case "-mlg":
                  System.out.println("Input level");
                  level = Integer.parseInt(scanner.next());
                  entityManager.getMagesWithLevelGreaterThen(level);
                  break;
              case "-thl":
                  System.out.println("Input height");
                  height = Integer.parseInt(scanner.next());
                  entityManager.getTowersWithHeightLessThan(height);
                  break;
              case "-mlgit":
                  System.out.println("Input tower name and level");
                  towerName = scanner.next();
                  level = Integer.parseInt(scanner.next());

                  entityManager.getMagesWithLevelHigherThanInTower(towerName, level);
                  break;
          }

        }
        scanner.close();
    }
}