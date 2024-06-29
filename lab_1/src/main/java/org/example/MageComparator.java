package org.example;

import java.util.Comparator;

public class MageComparator implements Comparator<Mage> {
    @Override
    public int compare(Mage firstMage, Mage scndMage) {
        if (firstMage.getLevel() != scndMage.getLevel()) {
            return firstMage.getLevel() - scndMage.getLevel();
        } else if (firstMage.getName().equals(scndMage.getName())) {
            return firstMage.getName().compareTo(scndMage.getName());
        } else {
            return (int) Math.round(firstMage.getPower()) - (int) Math.round(scndMage.getPower());
        }
    }
}
