using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public const float Ground_Bottom_Y = -1f;
    private EntityHero _Self;
    private List<EntityBase> m_HeroList = new List<EntityBase>();
    private List<EntityBase> m_EntityList = new List<EntityBase>();
    private Dictionary<GameObject, EntityBase> mObj2EntityList = new Dictionary<GameObject, EntityBase>();
    private List<EntityTowerBase> m_TowerList = new List<EntityTowerBase>();
    private float find_minDis;
    private float find_minDis1;
    private EntityBase find_target;
    private EntityBase find_mintarget;
    private EntityBase find_temp;
    private int find_i;
    private int find_imax;
    private List<EntityBase> findcanattacklist = new List<EntityBase>();
    private List<EntityBase> mCanHitList = new List<EntityBase>();
    private List<EntityBase> mRangeList = new List<EntityBase>();
    private const float ArrowEjectDistance = 7.5f;
    private EntityBase eject_temp;
    private EntityBase entitybase_temp;
    private List<EntityBase> rounds_list = new List<EntityBase>();
    private EntityBase rounds_temp;
    private List<EntityBase> round_list = new List<EntityBase>();
    private EntityBase round_temp;
    private float near_min;
    private float near_dis;
    private EntityBase near_entity;
    private EntityBase near_entitytemp;
    private EntityBase entity_random;
    private EntityBase entity_randomtemp;
    private List<EntityBase> Sector_list = new List<EntityBase>();
    private EntityBase Sector_e;
    private EntityBase entity_child;
    private List<EntityBabyBase> m_BabyList = new List<EntityBabyBase>();
    private Dictionary<string, DivideData> mDivideList = new Dictionary<string, DivideData>();
    private List<EntityPartBodyBase> m_PartBodyList = new List<EntityPartBodyBase>();

    public void Add(EntityBase entity)
    {
        if (!this.m_EntityList.Contains(entity))
        {
            this.m_EntityList.Add(entity);
        }
    }

    public void AddDivide(string divideid, DivideTransfer transfer)
    {
        if (this.mDivideList.TryGetValue(divideid, out DivideData data))
        {
            data.count++;
        }
        else
        {
            data = new DivideData {
                DivideID = divideid,
                count = 1,
                charid = transfer.charid,
                entitytype = transfer.entitytype
            };
            this.mDivideList.Add(divideid, data);
        }
    }

    public void AddHero(EntityBase hero)
    {
        this.m_HeroList.Add(hero);
    }

    public void AddTower(EntityTowerBase tower)
    {
        if (!this.m_TowerList.Contains(tower))
        {
            this.m_TowerList.Add(tower);
        }
        else
        {
            object[] args = new object[] { tower.name };
            SdkManager.Bugly_Report("EntityManager.cs", Utils.FormatString("AddTower is already contains {0}", args));
        }
    }

    public void DeInit()
    {
        this.MonstersClear();
        if (this.Self != null)
        {
            this.Self.DeInit();
            Object.Destroy(this.Self.gameObject);
        }
        this.m_HeroList.Clear();
        this.mCanHitList.Clear();
        this.mRangeList.Clear();
        this.Sector_list.Clear();
        this.mObj2EntityList.Clear();
        this.round_list.Clear();
        this.rounds_list.Clear();
        this.DeInitBabies();
        this.DeInitPartBodies();
    }

    private void DeInitBabies()
    {
        for (int i = this.m_BabyList.Count - 1; i >= 0; i--)
        {
            this.m_BabyList[i].DeInit();
        }
        this.m_BabyList.Clear();
    }

    private void DeInitPartBodies()
    {
        for (int i = this.m_PartBodyList.Count - 1; i >= 0; i--)
        {
            this.m_PartBodyList[i].DeInit();
        }
        this.m_PartBodyList.Clear();
    }

    public EntityBase FindArrowEject(EntityBase entity)
    {
        this.mCanHitList.Clear();
        this.mRangeList.Clear();
        int num = 0;
        int count = this.m_EntityList.Count;
        while (num < count)
        {
            this.eject_temp = this.m_EntityList[num];
            float num3 = Vector3.Distance(this.eject_temp.transform.position, entity.position);
            if ((((this.eject_temp != null) && (this.eject_temp != entity)) && (this.eject_temp.gameObject.activeInHierarchy && (this.eject_temp.Type == entity.Type))) && ((!this.eject_temp.GetIsDead() && this.eject_temp.GetIsInCamera()) && (num3 < 7.5f)))
            {
                if (GameLogic.GetCanHit(entity, this.eject_temp))
                {
                    this.mCanHitList.Add(this.eject_temp);
                }
                this.mRangeList.Add(this.eject_temp);
            }
            num++;
        }
        if (this.mCanHitList.Count > 0)
        {
            return this.mCanHitList[Random.Range(0, this.mCanHitList.Count)];
        }
        if (this.mRangeList.Count > 0)
        {
            return this.mRangeList[Random.Range(0, this.mRangeList.Count)];
        }
        return null;
    }

    public EntityBase FindCanAttackRandom(EntityBase self)
    {
        List<EntityBase> targetList = this.GetTargetList(self, false);
        this.findcanattacklist.Clear();
        this.find_minDis = 2.147484E+09f;
        this.find_minDis1 = 2.147484E+09f;
        this.find_target = null;
        this.find_mintarget = null;
        this.find_i = 0;
        this.find_imax = targetList.Count;
        while (this.find_i < this.find_imax)
        {
            this.find_temp = targetList[this.find_i];
            if ((((this.find_temp != null) && this.find_temp.gameObject.activeInHierarchy) && (!this.find_temp.GetIsDead() && this.find_temp.GetIsInCamera())) && (this.find_temp.GetColliderEnable() && this.find_temp.GetMeshShow()))
            {
                this.findcanattacklist.Add(this.find_temp);
            }
            this.find_i++;
        }
        if (this.findcanattacklist.Count == 0)
        {
            return null;
        }
        int num = GameLogic.Random(0, this.findcanattacklist.Count);
        return this.findcanattacklist[num];
    }

    public EntityBase FindTargetExclude(EntityBase exclude)
    {
        this.find_minDis = 2.147484E+09f;
        this.find_minDis1 = 2.147484E+09f;
        this.find_target = null;
        this.find_mintarget = null;
        this.find_i = 0;
        this.find_imax = this.m_EntityList.Count;
        while (this.find_i < this.find_imax)
        {
            this.find_temp = this.m_EntityList[this.find_i];
            if ((((this.find_temp != null) && this.find_temp.gameObject.activeInHierarchy) && ((this.find_temp != exclude) && !this.find_temp.GetIsDead())) && ((this.find_temp.GetIsInCamera() && !GameLogic.IsSameTeam(this.find_temp, GameLogic.Self)) && this.find_temp.GetColliderEnable()))
            {
                float num = Vector3.Distance(this.find_temp.transform.position, this.Self.position);
                if ((num < this.find_minDis) && GameLogic.GetCanHit(this.Self, this.find_temp))
                {
                    this.find_target = this.find_temp;
                    this.find_minDis = num;
                }
                if (num < this.find_minDis1)
                {
                    this.find_mintarget = this.find_temp;
                    this.find_minDis1 = num;
                }
            }
            this.find_i++;
        }
        return this.find_target;
    }

    public EntityBase FindTargetInCamera()
    {
        this.find_minDis = 2.147484E+09f;
        this.find_minDis1 = 2.147484E+09f;
        this.find_target = null;
        this.find_mintarget = null;
        this.find_i = 0;
        this.find_imax = this.m_EntityList.Count;
        while (this.find_i < this.find_imax)
        {
            this.find_temp = this.m_EntityList[this.find_i];
            if ((((this.find_temp != null) && this.find_temp.gameObject.activeInHierarchy) && (!this.find_temp.GetIsDead() && this.find_temp.GetIsInCamera())) && ((!GameLogic.IsSameTeam(this.find_temp, GameLogic.Self) && this.find_temp.GetColliderEnable()) && (this.find_temp.GetMeshShow() && (this.find_temp.position.y >= -1f))))
            {
                float num = Vector3.Distance(this.find_temp.position, this.Self.position);
                if ((num < this.find_minDis) && GameLogic.GetCanHit(this.Self, this.find_temp))
                {
                    this.find_target = this.find_temp;
                    this.find_minDis = num;
                }
                if (num < this.find_minDis1)
                {
                    this.find_mintarget = this.find_temp;
                    this.find_minDis1 = num;
                }
            }
            this.find_i++;
        }
        if (this.find_target != null)
        {
        }
        if (((this.find_target == null) && (this.find_mintarget != null)) && (this.find_mintarget.position.y >= -1f))
        {
            return this.find_mintarget;
        }
        return this.find_target;
    }

    public int GetActiveEntityCount()
    {
        int num = 0;
        int num2 = 0;
        int count = this.m_EntityList.Count;
        while (num2 < count)
        {
            if (((this.m_EntityList[num2] != null) && this.m_EntityList[num2].gameObject.activeInHierarchy) && !this.m_EntityList[num2].GetIsDead())
            {
                num++;
            }
            num2++;
        }
        return num;
    }

    public bool GetDivideDead(string divideid, out List<int> goodlist, out EntityType entitytype)
    {
        if (this.mDivideList.TryGetValue(divideid, out DivideData data))
        {
            if (data.count == 0)
            {
                goodlist = data.goodlist;
                this.mDivideList.Remove(divideid);
                entitytype = data.entitytype;
                return true;
            }
            goodlist = null;
            entitytype = EntityType.Invalid;
            return false;
        }
        goodlist = null;
        entitytype = EntityType.Invalid;
        return true;
    }

    public List<EntityBase> GetEntities() => 
        this.m_EntityList;

    public EntityBase GetEntityBase(GameObject o)
    {
        int num = 0;
        int count = this.m_EntityList.Count;
        while (num < count)
        {
            this.entitybase_temp = this.m_EntityList[num];
            if (this.entitybase_temp.gameObject == o)
            {
                return this.entitybase_temp;
            }
            num++;
        }
        return null;
    }

    public EntityBase GetEntityByChild(GameObject o)
    {
        if (!this.mObj2EntityList.TryGetValue(o, out this.entity_child))
        {
            this.entity_child = o.GetComponent<EntityBase>();
            this.mObj2EntityList.Add(o, this.entity_child);
        }
        return this.entity_child;
    }

    public int GetEntityCount() => 
        this.m_EntityList.Count;

    public EntityBase GetNearEntity(BulletBase bullet, bool sameteam)
    {
        List<EntityBase> targetList = this.GetTargetList(bullet.m_Entity, sameteam);
        this.near_min = 2.147484E+09f;
        this.near_entity = null;
        int num = 0;
        int count = targetList.Count;
        while (num < count)
        {
            this.near_entitytemp = targetList[num];
            this.near_dis = Vector3.Distance(this.near_entitytemp.position, bullet.transform.position);
            if (((this.near_entitytemp != null) && this.near_entitytemp.gameObject.activeInHierarchy) && (!this.near_entitytemp.GetIsDead() && (this.near_dis < this.near_min)))
            {
                this.near_entity = this.near_entitytemp;
                this.near_min = this.near_dis;
            }
            num++;
        }
        return this.near_entity;
    }

    public EntityBase GetNearEntity(EntityBase self, float range, bool sameteam)
    {
        List<EntityBase> targetList = this.GetTargetList(self, sameteam);
        this.near_min = 2.147484E+09f;
        this.near_entity = null;
        int num = 0;
        int count = targetList.Count;
        while (num < count)
        {
            this.near_entitytemp = targetList[num];
            this.near_dis = Vector3.Distance(this.near_entitytemp.position, self.position);
            if ((((this.near_entitytemp != null) && (this.near_entitytemp != self)) && (this.near_entitytemp.gameObject.activeInHierarchy && !this.near_entitytemp.GetIsDead())) && ((this.near_dis < range) && (this.near_dis < this.near_min)))
            {
                this.near_entity = this.near_entitytemp;
                this.near_min = this.near_dis;
            }
            num++;
        }
        return this.near_entity;
    }

    public EntityBase GetNearTarget(EntityBase self)
    {
        List<EntityBase> targetList = this.GetTargetList(self, false);
        this.near_min = 2.147484E+09f;
        this.near_entity = null;
        int num = 0;
        int count = targetList.Count;
        while (num < count)
        {
            this.near_entitytemp = targetList[num];
            this.near_dis = Vector3.Distance(this.near_entitytemp.position, self.position);
            if ((((this.near_entitytemp != null) && (this.near_entitytemp != self)) && (this.near_entitytemp.gameObject.activeInHierarchy && (this.near_dis < this.near_min))) && ((!this.near_entitytemp.GetIsDead() && (this.near_entitytemp.position.y >= 0f)) && this.near_entitytemp.GetMeshShow()))
            {
                this.near_entity = this.near_entitytemp;
                this.near_min = this.near_dis;
            }
            num++;
        }
        return this.near_entity;
    }

    public EntityBase GetRandomEntity(EntityBase self, float range, bool sameteam)
    {
        List<EntityBase> targetList = this.GetTargetList(self, sameteam);
        this.entity_random = null;
        int num = GameLogic.Random(0, targetList.Count);
        int num2 = 0;
        int count = targetList.Count;
        while (num2 < count)
        {
            this.entity_randomtemp = targetList[(num + num2) % count];
            float num4 = Vector3.Distance(this.entity_randomtemp.position, self.position);
            if ((((this.entity_randomtemp != null) && (this.entity_randomtemp != self)) && (this.entity_randomtemp.gameObject.activeInHierarchy && !this.entity_randomtemp.GetIsDead())) && (num4 < range))
            {
                this.entity_random = this.entity_randomtemp;
                break;
            }
            num2++;
        }
        return this.entity_random;
    }

    public List<EntityBase> GetRoundEntities(EntityBase entity, float range, bool haveself)
    {
        this.rounds_list = new List<EntityBase>();
        int num = 0;
        int count = this.m_EntityList.Count;
        while (num < count)
        {
            this.rounds_temp = this.m_EntityList[num];
            if ((((this.rounds_temp != null) && this.rounds_temp.gameObject.activeInHierarchy) && ((!haveself && (this.rounds_temp != entity)) || haveself)) && (!this.rounds_temp.GetIsDead() && (Vector3.Distance(this.rounds_temp.position, entity.position) < range)))
            {
                this.rounds_list.Add(this.rounds_temp);
            }
            num++;
        }
        return this.rounds_list;
    }

    public List<EntityBase> GetRoundSelfEntities(EntityBase self, float range, bool sameteam)
    {
        this.round_list.Clear();
        int num = 0;
        int count = this.m_EntityList.Count;
        while (num < count)
        {
            this.round_temp = this.m_EntityList[num];
            if ((((this.round_temp != null) && (this.round_temp.gameObject != null)) && ((this.round_temp != self) && this.round_temp.gameObject.activeInHierarchy)) && ((!this.round_temp.GetIsDead() && (GameLogic.IsSameTeam(this.round_temp, self) == sameteam)) && (Vector3.Distance(this.round_temp.position, self.position) < range)))
            {
                this.round_list.Add(this.round_temp);
            }
            num++;
        }
        return this.round_list;
    }

    public List<EntityBase> GetSectorEntities(EntityBase self, float range, float middleangle, float offsetangle, bool sameteam)
    {
        List<EntityBase> targetList = this.GetTargetList(self, sameteam);
        this.Sector_list.Clear();
        int num = 0;
        int count = targetList.Count;
        while (num < count)
        {
            this.Sector_e = targetList[num];
            float num3 = Vector3.Distance(this.Sector_e.position, self.position);
            if ((((this.Sector_e != null) && (this.Sector_e != self)) && (this.Sector_e.gameObject.activeInHierarchy && !this.Sector_e.GetIsDead())) && (num3 < range))
            {
                float num4 = Utils.getAngle(this.Sector_e.position.x - self.position.x, this.Sector_e.position.z - self.position.z);
                if (((MathDxx.Abs((float) (num4 - middleangle)) <= offsetangle) || (MathDxx.Abs((float) ((num4 - middleangle) + 360f)) <= offsetangle)) || (MathDxx.Abs((float) ((num4 - middleangle) - 360f)) <= offsetangle))
                {
                    this.Sector_list.Add(this.Sector_e);
                }
            }
            num++;
        }
        return this.Sector_list;
    }

    private List<EntityBase> GetTargetList(EntityBase self, bool issameteam)
    {
        if (GameLogic.IsSameTeam(self, GameLogic.Self) == issameteam)
        {
            return this.m_HeroList;
        }
        return this.m_EntityList;
    }

    public bool IsSelfObject(GameObject o) => 
        (o == this.Self.gameObject);

    public void MonstersClear()
    {
        for (int i = this.m_EntityList.Count - 1; i >= 0; i--)
        {
            this.m_EntityList[i].DeInit();
        }
        for (int j = this.m_TowerList.Count - 1; j >= 0; j--)
        {
            this.RemoveTower(this.m_TowerList[j]);
        }
        this.m_TowerList.Clear();
        this.m_EntityList.Clear();
    }

    public void Remove(EntityBase entity)
    {
        if (this.m_EntityList.Contains(entity))
        {
            entity.DeInit();
        }
        else if (entity == this._Self)
        {
            this._Self.DeInit();
        }
    }

    public void RemoveBaby(EntityBabyBase baby)
    {
        baby.DeInit();
        this.m_BabyList.Remove(baby);
    }

    public void RemoveDivide(string divideid)
    {
        if (this.mDivideList.TryGetValue(divideid, out DivideData data))
        {
            data.count--;
        }
    }

    public void RemoveLogic(EntityBase entity)
    {
        if (this.m_EntityList.Contains(entity))
        {
            this.m_EntityList.Remove(entity);
        }
    }

    public void RemovePartBody(EntityPartBodyBase partbody, bool gotonextroom = false)
    {
        partbody.bGotoRoomRemove = gotonextroom;
        partbody.DeInit();
        this.m_PartBodyList.Remove(partbody);
    }

    public void RemoveTower(EntityTowerBase tower)
    {
        if (this.m_TowerList.Contains(tower))
        {
            this.m_TowerList.Remove(tower);
            tower.DeInit();
        }
    }

    public void SetBaby(EntityBabyBase baby)
    {
        this.m_BabyList.Add(baby);
    }

    public void SetPartBody(EntityPartBodyBase partbody)
    {
        this.m_PartBodyList.Add(partbody);
    }

    public void SetSelf(EntityHero self)
    {
        this._Self = self;
        this.AddHero(self);
    }

    public EntityHero Self =>
        this._Self;

    public class DivideData
    {
        public string DivideID;
        public int charid;
        public EntityType entitytype;
        public int count;
        public List<int> goodlist;
    }

    public class DivideTransfer
    {
        public string divedeid;
        public int charid;
        public EntityType entitytype;
    }
}

