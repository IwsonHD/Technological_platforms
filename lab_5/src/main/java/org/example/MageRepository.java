package org.example;

import java.util.HashMap;
import java.util.Optional;

public class MageRepository {
    private HashMap<String, Mage> mageArmy;
    public MageRepository(){
        this.mageArmy = new HashMap<>();
    }
    public Optional<Mage> findMage(String name){
        Mage mage = this.mageArmy.get(name);
        if(mage == null) return Optional.empty();
        return Optional.of(mage);
    }

    public void delete(String name){
        if(!this.mageArmy.containsKey(name)){
           throw new IllegalArgumentException();
        }
        this.mageArmy.remove(name);
    }

    public void save(Mage mage){
        if(this.mageArmy.containsKey(mage.getName())){
           throw new IllegalArgumentException();
        }
        this.mageArmy.put(mage.getName(), mage);
    }



}
