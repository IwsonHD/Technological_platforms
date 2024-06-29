package org.example;

import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class MageRepositoryTest {

    private static MageRepository mageRepository;
    @BeforeAll
    public static void setup(){
        mageRepository = new MageRepository();
    }

    @Test
    void deleteNonExisting(){
        String nonExistingMageName = "NoneExistingMage";
        assertThrows(IllegalArgumentException.class, () -> {
            mageRepository.delete(nonExistingMageName);
        });
    }
    @Test
    void deleteExisting(){
        Mage mage = new Mage("ExistingMage", 10);
        mageRepository.save(mage);
        mageRepository.delete("ExistingMage");
        assertTrue(mageRepository.findMage("ExistingMage").isEmpty());
    }

    @Test
    void findExisting(){
        mageRepository.save(new Mage("ExistingMage", 10));
        assertFalse(mageRepository.findMage("ExistingMage").isEmpty());
        mageRepository.delete("ExistingMage");
    }

    @Test
    void saveExisting(){
        mageRepository.save(new Mage("ExistingMage",10));
        assertThrows(IllegalArgumentException.class,()->{
            mageRepository.save(new Mage("ExistingMage", 10));
        });
        mageRepository.delete("ExistingMage");
    }

}