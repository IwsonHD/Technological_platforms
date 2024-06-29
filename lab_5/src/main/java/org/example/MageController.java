package org.example;

import com.fasterxml.jackson.core.JsonProcessingException;

import java.util.Optional;

public class MageController {
    private MageRepository mageRepository;

    public MageController(MageRepository mageRepository){
        this.mageRepository = mageRepository;
    }

    public String find(String name){
        Optional<Mage> mage = this.mageRepository.findMage(name);
        if(mage.isEmpty()) return "not found";
        return mage.get().toString();
    }

    public String delete(String name){
        try{
            this.mageRepository.delete(name);
            return "done";
        }catch(IllegalArgumentException e){
            return "not found";
        }
    }

    public String create(String name, String level){
        try {
            int mageLevel = Integer.parseInt(level);
            this.mageRepository.save(new Mage(name,mageLevel));
        }catch (IllegalArgumentException e){
            return "bad request";
        }
        return "done";
    }



}
