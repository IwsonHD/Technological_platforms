package org.example;

import java.util.*;
import java.lang.Math;

public class Mage implements Comparable<Mage> {
    private String name;
    private int level;
    private double power;
    private Set<Mage> apprentices;
    private int mode;

    Mage(String name, int level, double power, int mode){
        this.name = name;
        this.level = level;
        this.power = power;
        if(mode == 0) apprentices = new HashSet<Mage>();
        else if(mode == 1) apprentices = new TreeSet<Mage>();
        else apprentices = new TreeSet<Mage>(new MageComparator());
    }



    @Override
    public int compareTo(Mage other){
        if(this.name.equals(other.getName())){
            return this.name.compareTo(other.name);
        }else if(this.level != other.getLevel()){
            return this.level - other.getLevel();
        }else{
            return (int) Math.round(this.power) - (int) Math.round(other.getPower());
        }
    }

    @Override
    public boolean equals(Object o){
        if (this == o) return true;
        if (o == null) return false;
        if (this.getClass() != o.getClass()) return false;
        return this.level == ((Mage) o).getLevel()
                && this.name.equals(((Mage) o).name)
                && this.power == ((Mage) o).power;
    }
    @Override
    public int hashCode(){
        return this.level + this.name.hashCode()*(int)this.power;
    }

    @Override
    public String toString(){
        return "Mage{name=" + this.name + ", level=" + this.level + ", power=" + this.power + "}";
    }

    public String getName(){
        return name;
    }

    public int getLevel() {
        return level;
    }

    public Set<Mage> getApprentices() {
        return apprentices;
    }

    public double getPower() {
        return power;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setPower(double power) {
        this.power = power;
    }
    public void addApprentice(Mage apprentice){
        apprentices.add(apprentice);
    }
    public void showApprentices(){
        //wraper
        logic_showApr(0, new HashSet<Mage>());
    }
    private void logic_showApr(int depth, HashSet<Mage> visited){
        visited.add(this);
        for(int i = 0; i < depth ; i++){
            System.out.print("-");
        }
        System.out.println(this.toString());
        for(Mage mage : this.apprentices){
            if(!visited.contains(mage))
                mage.logic_showApr(depth + 1, visited);
        }
    }

    public void showStatistics() {
        Map<Mage, Integer> stats;
        //wraper
        if(this.mode == 0){
            stats = new HashMap<Mage, Integer>();
            logic_showStat(new HashSet<Mage>(), stats);
        }else if(this.mode == 1){
            stats = new TreeMap<Mage, Integer>();
            logic_showStat(new HashSet<Mage>(), stats);
        }else{
            stats = new TreeMap<Mage, Integer>(new MageComparator());
            logic_showStat(new HashSet<Mage>(),stats);
        }

        for(Map.Entry<Mage,Integer> stat : stats.entrySet()){
            System.out.println("Mage: " + stat.getKey().toString() + " has " + stat.getValue().toString() + " apprentices");
        }

    }

    private void logic_showStat(HashSet<Mage> visited, Map<Mage, Integer> stats){

        stats.put(this, this.getApprentices().size());
        visited.add(this);
        for( Mage mage : this.apprentices){
            if(!visited.contains(mage)){
                mage.logic_showStat(visited, stats);
                for(Mage mage1 : mage.getApprentices()){
                    if(!this.getApprentices().contains(mage1)) stats.put(this, stats.get(this) + 1);
                }
            }
        }

    }



}


