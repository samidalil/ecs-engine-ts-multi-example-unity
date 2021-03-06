using System.Collections.Generic;

namespace PA.Data
{
    /// <summary>
    /// Le format d'une information d'update sur un composant d'entité
    /// </summary>
    public class ComponentInfo
    {
        public int id;
        public MixedFieldsComponent data;
    }

    /// <summary>
    /// Le format d'une information d'update sur une entité
    /// </summary>
    public class EntityInfo
    {
        public int id;
        public List<ComponentInfo> components;
    }

    /// <summary>
    /// Le format d'une update envoyée par le serveur
    /// </summary>
    public class StateUpdateInfo
    {
        public List<EntityInfo> data;
        public int frame;
    }

    /// <summary>
    /// Le format d'une information d'update sur une entité
    /// </summary>
    public class EntityDiffInfo
    {
        public int id;
        public NetworkEventType eventType;
        public List<ComponentInfo> components;
    }

    /// <summary>
    /// Le format d'une update envoyée par le serveur
    /// </summary>
    public class DiffUpdateInfo
    {
        public List<EntityDiffInfo> data;
        public int frame;
    }

    /// <summary>
    /// Le format d'une information de création d'entité
    /// </summary>
    public class EntityCreatedInfo
    {
        public int id;
    }

    /// <summary>
    /// Le format d'une information d'initialisation de connexion au serveur
    /// </summary>
    public class InitInfo : StateUpdateInfo
    {
        public int assignedId;
    }
}
