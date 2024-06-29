package org.example;

import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.mockito.invocation.InvocationOnMock;
import org.mockito.stubbing.Answer;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class MageControllerTest {
    private static MageController mageController;
    private static MageRepository mageRepositoryMock;
    @BeforeAll
    public static void setup(){
        mageRepositoryMock = mock(MageRepository.class);
        mageController = new MageController(mageRepositoryMock);
    }

    @Test
    public void deleteExisting(){
        String existingMage = "ExistingMage";
        doNothing().when(mageRepositoryMock).delete(existingMage);
        assertEquals("done", mageController.delete(existingMage));
    }

    @Test
    public void deleteNonExisting(){
        String notExistingMage = "NotExistingMage";
        doThrow(new IllegalArgumentException()).when(mageRepositoryMock).delete(notExistingMage);
        assertEquals("not found", mageController.delete(notExistingMage));
    }

    @Test
    public void findNonExisting(){
        String nonExistingMage = "NonExistingMage";
        when(mageRepositoryMock.findMage(nonExistingMage)).thenReturn(Optional.empty());
        assertEquals("not found", mageController.find(nonExistingMage));
    }

    @Test
    public void findExisting(){
        String existingMageName = "ExistingMage";
        Mage existingMage = new Mage(existingMageName, 10);
        when(mageRepositoryMock.findMage(existingMageName)).thenReturn(Optional.of(existingMage));
        assertEquals(existingMage.toString(), mageController.find(existingMageName));
    }

    @Test
    public void saveCorrectlyNonExisting(){
        String nonExistingMageName = "NonExistingMage";
        int levelOfNoneExistingMage = 10;
        Mage existingMage = new Mage(nonExistingMageName, levelOfNoneExistingMage);
        doNothing().when(mageRepositoryMock).save(existingMage);
        assertEquals("done", mageController.create(nonExistingMageName,
                Integer.toString(levelOfNoneExistingMage)));
    }

    @Test
    public void saveExisting(){
        // Given
        String existingMageName = "ExistingMage";
        int levelOfExistingMage = 10;
        Mage existingMage = new Mage(existingMageName, levelOfExistingMage);

        // When
        doAnswer(new Answer<Void>() {
            public Void answer(InvocationOnMock invocation) {
                Mage mageArgument = (Mage) invocation.getArguments()[0];
                if (mageArgument.getName().equals(existingMageName)) {
                    throw new IllegalArgumentException();
                }
                return null;
            }
        }).when(mageRepositoryMock).save(any(Mage.class));

        // Then
        assertEquals("bad request", mageController.create(existingMageName, Integer.toString(levelOfExistingMage)));
    }
}