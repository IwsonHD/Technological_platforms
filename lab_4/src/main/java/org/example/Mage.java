package org.example;


import javax.persistence.*;

@Entity
public class Mage {
    @Id
    private String name;

    private int level;

    @ManyToOne
    private Tower tower;

    public void setName(String name) {
        this.name = name;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public void setTower(Tower tower) {
        this.tower = tower;
    }

    public String getName() {
        return name;
    }

    public int getLevel() {
        return level;
    }

    public Tower getTower() {
        return tower;
    }
}
