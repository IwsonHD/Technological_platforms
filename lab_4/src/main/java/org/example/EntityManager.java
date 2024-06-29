package org.example;


import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;
import org.hibernate.query.Query;

import java.util.List;

public class EntityManager {
  private static final SessionFactory sessionFactory;
  static{
      sessionFactory = new Configuration().configure().buildSessionFactory();
  }

  public void addNewMage(String name, int level, String towerName){
      Session session = null;
      try {
          session = sessionFactory.openSession();
          session.beginTransaction();

          Tower tower = session.get(Tower.class, towerName);

          if(tower == null){
              throw new Exception("No such tower exists");
          }

          Mage newMage = new Mage();
          newMage.setLevel(level);
          newMage.setName(name);
          newMage.setTower(tower);
          session.save(newMage);
          tower.getMages().add(newMage);
          session.update(tower);
          session.getTransaction().commit();
          System.out.println("A new mage by the name of " + name +" has been successfully added to" +
                  "the " + towerName + " tower.");
      } catch(Exception e) {
          if(session != null && session.getTransaction().isActive()){
              session.getTransaction().rollback();
          }
          System.err.println("Error: " + e.getMessage());
      } finally {
          if(session != null){
              session.close();
          }
      }

  }

  public void addNewTower(String towerName, int height){
      Session session = null;

      try{
          session = sessionFactory.openSession();
          session.beginTransaction();
          Tower newTower = new Tower();
          newTower.setHeight(height);
          newTower.setName(towerName);
          session.save(newTower);
          session.getTransaction().commit();
          System.out.println("A new tower by the name of " + towerName + " has been successfully added.");
      } catch(Exception e) {
          if(session != null && session.getTransaction().isActive()){
              session.getTransaction().rollback();
          }
          System.err.println("Error: " + e.getMessage());
      } finally {
          if(session != null){
              session.close();
          }
      }


  }

  public void removeTower(String towerName){
      Session session = null;
      try{
          session = sessionFactory.openSession();
          session.beginTransaction();
          Tower entry = session.get(Tower.class, towerName);

          if(entry == null){
              throw new Exception("No such tower exists");
          }

          if(!entry.getMages().isEmpty()){
              throw new Exception("Cannot delete a tower that has mages assignet to it");
          }

          session.delete(entry);
          session.getTransaction().commit();
          System.out.println("Tower has been deleted succesfully");
      } catch(Exception e) {
          if(session != null && session.getTransaction().isActive()){
              session.getTransaction().rollback();
          }
          System.err.println("Error: " + e.getMessage());
      } finally {
          if(session != null){
              session.close();
          }
      }
  }

    public void removeMage(String mageName) {
        Session session = null;
        try {
            session = sessionFactory.openSession();
            session.beginTransaction();

            // Sprawdź, czy mag jest powiązany z jakąkolwiek wieżą w tabeli TOWER_MAGE
            Query<Tower> query = session.createQuery("SELECT tower FROM Tower tower JOIN tower.mages mage WHERE mage.name = :mageName", Tower.class);
            query.setParameter("mageName", mageName);
            List<Tower> towers = query.getResultList();

            // Usuń powiązania maga z wieżami
            for (Tower tower : towers) {
                tower.getMages().removeIf(mage -> mage.getName().equals(mageName));
                session.update(tower);
            }

            // Usuń maga
            Mage mage = session.get(Mage.class, mageName);
            if (mage == null) {
                throw new Exception("No such mage exists");
            }
            session.delete(mage);
            session.getTransaction().commit();
            System.out.println("Mage has succesfully been deleted");
        } catch (Exception e) {
            if (session != null && session.getTransaction().isActive()) {
                session.getTransaction().rollback();
            }
            System.err.println("Error: " + e.getMessage());
        } finally {
            if (session != null) {
                session.close();
            }
        }
    }

  public void showMagesByTowers(){
    Session session = null;
    try{
        session = sessionFactory.openSession();
        List<Tower> towers = session.createQuery("SELECT a FROM Tower a", Tower.class).getResultList();
        for(Tower tower : towers){

            System.out.println("Tower " + tower.getName() + ":");

            for(Mage mage : tower.getMages()){
                System.out.println("    Mage - " + mage.getName());
            }
        }
    }catch(Exception e) {
        if(session != null && session.getTransaction().isActive()){
            session.getTransaction().rollback();
        }
        System.err.println("Error: " + e.getMessage());
    } finally {
        if(session != null){
            session.close();
        }
    }
  }

  public void getMagesWithLevelGreaterThen(int level){
      Session session = null;
      try{
          session = sessionFactory.openSession();
          Query<Mage> query = session.createQuery("FROM Mage WHERE level > :level", Mage.class);
          query.setParameter("level", level);
          System.out.println("Mages with level higher then " + level + ":");
        for(Mage mage : query.getResultList()){
            System.out.println("    Mage " +mage.getName() + " with level " + mage.getLevel());
        }
      }catch(Exception e) {
          if(session != null && session.getTransaction().isActive()){
              session.getTransaction().rollback();
          }
          System.err.println("Error: " + e.getMessage());
      } finally {
          if(session != null){
              session.close();
          }
      }

  }

  public void getTowersWithHeightLessThan(int height){
      Session session = null;
      try{
          session = sessionFactory.openSession();
          Query<Tower> query = session.createQuery("FROM Tower WHERE height < :height", Tower.class);
          query.setParameter("height", height);
          System.out.println("Towers with a heigth lower then " + height);
          for(Tower tower : query.getResultList()){
              System.out.println("     Tower " + tower.getName() + " with height " + tower.getHeight());
          }


      }catch(Exception e) {
          if(session != null && session.getTransaction().isActive()){
              session.getTransaction().rollback();
          }
          System.err.println("Error: " + e.getMessage());
      } finally {
          if(session != null){
              session.close();
          }
      }
  }

  public void getMagesWithLevelHigherThanInTower(String towerName, int level){
      Session session = null;
      try{
          session = sessionFactory.openSession();
          Query<Mage> query = session.createQuery("SELECT mage FROM Mage mage JOIN mage.tower tower WHERE tower.name = :towerName AND mage.level > :level", Mage.class);
          query.setParameter("towerName", towerName);
          query.setParameter("level", level);
          System.out.println("Mage with level lower then " + level + " from the tower " + towerName + ":");
          for(Mage mage : query.getResultList()){
              System.out.println("     Mage " + mage.getName() + " with level " + mage.getLevel());
          }

      }catch(Exception e) {
          if(session != null && session.getTransaction().isActive()){
              session.getTransaction().rollback();
          }
          System.err.println("Error: " + e.getMessage());
      } finally {
          if(session != null){
              session.close();
          }
      }
  }



}
