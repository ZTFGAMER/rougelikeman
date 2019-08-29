namespace TableTool
{
    using System;
    using System.Runtime.CompilerServices;

    public class LocalModelManager
    {
        private static volatile LocalModelManager _Instance;
        private Goods_foodModel _Goods_foodModel;
        private Equip_equipModel _Equip_equipModel;
        private Skill_slotoutcostModel _Skill_slotoutcostModel;
        private Config_configModel _Config_configModel;
        private Box_SilverNormalBoxModel _Box_SilverNormalBoxModel;
        private Skill_slotinModel _Skill_slotinModel;
        private Room_eventgameturnModel _Room_eventgameturnModel;
        private Operation_moveModel _Operation_moveModel;
        private Character_BabyModel _Character_BabyModel;
        private Shop_itemModel _Shop_itemModel;
        private Character_CharModel _Character_CharModel;
        private Language_lauguageModel _Language_lauguageModel;
        private Skill_dropinModel _Skill_dropinModel;
        private Drop_FakeDropModel _Drop_FakeDropModel;
        private Room_eventangelskillModel _Room_eventangelskillModel;
        private Box_ChapterBoxModel _Box_ChapterBoxModel;
        private Equip_UpgradeModel _Equip_UpgradeModel;
        private Stage_Level_activityModel _Stage_Level_activityModel;
        private Character_LevelModel _Character_LevelModel;
        private Room_eventdemontext2loseModel _Room_eventdemontext2loseModel;
        private Achieve_AchieveModel _Achieve_AchieveModel;
        private Sound_soundModel _Sound_soundModel;
        private Box_ActivityModel _Box_ActivityModel;
        private Fx_fxModel _Fx_fxModel;
        private Shop_MysticShopModel _Shop_MysticShopModel;
        private Curve_curveModel _Curve_curveModel;
        private Equip2_equip2Model _Equip2_equip2Model;
        private Stage_Level_chapter12Model _Stage_Level_chapter12Model;
        private Stage_Level_chapter13Model _Stage_Level_chapter13Model;
        private Stage_Level_chapter10Model _Stage_Level_chapter10Model;
        private Stage_Level_chapter11Model _Stage_Level_chapter11Model;
        private Buff_aloneModel _Buff_aloneModel;
        private Goods_goodsModel _Goods_goodsModel;
        private Box_SilverBoxModel _Box_SilverBoxModel;
        private Room_soldierupModel _Room_soldierupModel;
        private Stage_Level_chapter9Model _Stage_Level_chapter9Model;
        private Test_AttrValueModel _Test_AttrValueModel;
        private Stage_Level_chapter7Model _Stage_Level_chapter7Model;
        private Stage_Level_chapter8Model _Stage_Level_chapter8Model;
        private Stage_Level_chapter5Model _Stage_Level_chapter5Model;
        private Stage_Level_chapter6Model _Stage_Level_chapter6Model;
        private Stage_Level_chapter3Model _Stage_Level_chapter3Model;
        private Room_roomModel _Room_roomModel;
        private Room_eventdemontext2skillModel _Room_eventdemontext2skillModel;
        private Stage_Level_chapter4Model _Stage_Level_chapter4Model;
        private Stage_Level_chapter1Model _Stage_Level_chapter1Model;
        private Room_colorstyleModel _Room_colorstyleModel;
        private Stage_Level_chapter2Model _Stage_Level_chapter2Model;
        private Stage_Level_stagechapterModel _Stage_Level_stagechapterModel;
        private Shop_MysticShopShowModel _Shop_MysticShopShowModel;
        private Drop_harvestModel _Drop_harvestModel;
        private Box_TimeBoxModel _Box_TimeBoxModel;
        private Preload_loadModel _Preload_loadModel;
        private Room_levelModel _Room_levelModel;
        private Shop_ReadyShopModel _Shop_ReadyShopModel;
        private Skill_superModel _Skill_superModel;
        private Shop_ShopModel _Shop_ShopModel;
        private Soldier_standardModel _Soldier_standardModel;
        private Skill_aloneModel _Skill_aloneModel;
        private Goods_waterModel _Goods_waterModel;
        private Soldier_soldierModel _Soldier_soldierModel;
        private Beat_beatModel _Beat_beatModel;
        private Weapon_weaponModel _Weapon_weaponModel;
        private Skill_skillModel _Skill_skillModel;
        private Skill_greedyskillModel _Skill_greedyskillModel;
        private Shop_GoldModel _Shop_GoldModel;
        private Skill_slotfirstModel _Skill_slotfirstModel;
        private Exp_expModel _Exp_expModel;
        private Drop_GoldModel _Drop_GoldModel;
        private Drop_DropModel _Drop_DropModel;
        private Stage_Level_activitylevelModel _Stage_Level_activitylevelModel;
        private Skill_slotoutModel _Skill_slotoutModel;

        private LocalModelManager()
        {
        }

        public void InitializeAll()
        {
            Goods_foodModel model = this.Goods_food;
            Equip_equipModel model2 = this.Equip_equip;
            Skill_slotoutcostModel model3 = this.Skill_slotoutcost;
            Config_configModel model4 = this.Config_config;
            Box_SilverNormalBoxModel model5 = this.Box_SilverNormalBox;
            Skill_slotinModel model6 = this.Skill_slotin;
            Room_eventgameturnModel model7 = this.Room_eventgameturn;
            Operation_moveModel model8 = this.Operation_move;
            Character_BabyModel model9 = this.Character_Baby;
            Shop_itemModel model10 = this.Shop_item;
            Character_CharModel model11 = this.Character_Char;
            Language_lauguageModel model12 = this.Language_lauguage;
            Skill_dropinModel model13 = this.Skill_dropin;
            Drop_FakeDropModel model14 = this.Drop_FakeDrop;
            Room_eventangelskillModel model15 = this.Room_eventangelskill;
            Box_ChapterBoxModel model16 = this.Box_ChapterBox;
            Equip_UpgradeModel model17 = this.Equip_Upgrade;
            Stage_Level_activityModel model18 = this.Stage_Level_activity;
            Character_LevelModel model19 = this.Character_Level;
            Room_eventdemontext2loseModel model20 = this.Room_eventdemontext2lose;
            Achieve_AchieveModel model21 = this.Achieve_Achieve;
            Sound_soundModel model22 = this.Sound_sound;
            Box_ActivityModel model23 = this.Box_Activity;
            Fx_fxModel model24 = this.Fx_fx;
            Shop_MysticShopModel model25 = this.Shop_MysticShop;
            Curve_curveModel model26 = this.Curve_curve;
            Equip2_equip2Model model27 = this.Equip2_equip2;
            Stage_Level_chapter12Model model28 = this.Stage_Level_chapter12;
            Stage_Level_chapter13Model model29 = this.Stage_Level_chapter13;
            Stage_Level_chapter10Model model30 = this.Stage_Level_chapter10;
            Stage_Level_chapter11Model model31 = this.Stage_Level_chapter11;
            Buff_aloneModel model32 = this.Buff_alone;
            Goods_goodsModel model33 = this.Goods_goods;
            Box_SilverBoxModel model34 = this.Box_SilverBox;
            Room_soldierupModel model35 = this.Room_soldierup;
            Stage_Level_chapter9Model model36 = this.Stage_Level_chapter9;
            Test_AttrValueModel model37 = this.Test_AttrValue;
            Stage_Level_chapter7Model model38 = this.Stage_Level_chapter7;
            Stage_Level_chapter8Model model39 = this.Stage_Level_chapter8;
            Stage_Level_chapter5Model model40 = this.Stage_Level_chapter5;
            Stage_Level_chapter6Model model41 = this.Stage_Level_chapter6;
            Stage_Level_chapter3Model model42 = this.Stage_Level_chapter3;
            Room_roomModel model43 = this.Room_room;
            Room_eventdemontext2skillModel model44 = this.Room_eventdemontext2skill;
            Stage_Level_chapter4Model model45 = this.Stage_Level_chapter4;
            Stage_Level_chapter1Model model46 = this.Stage_Level_chapter1;
            Room_colorstyleModel model47 = this.Room_colorstyle;
            Stage_Level_chapter2Model model48 = this.Stage_Level_chapter2;
            Stage_Level_stagechapterModel model49 = this.Stage_Level_stagechapter;
            Shop_MysticShopShowModel model50 = this.Shop_MysticShopShow;
            Drop_harvestModel model51 = this.Drop_harvest;
            Box_TimeBoxModel model52 = this.Box_TimeBox;
            Preload_loadModel model53 = this.Preload_load;
            Room_levelModel model54 = this.Room_level;
            Shop_ReadyShopModel model55 = this.Shop_ReadyShop;
            Skill_superModel model56 = this.Skill_super;
            Shop_ShopModel model57 = this.Shop_Shop;
            Soldier_standardModel model58 = this.Soldier_standard;
            Skill_aloneModel model59 = this.Skill_alone;
            Goods_waterModel model60 = this.Goods_water;
            Soldier_soldierModel model61 = this.Soldier_soldier;
            Beat_beatModel model62 = this.Beat_beat;
            Weapon_weaponModel model63 = this.Weapon_weapon;
            Skill_skillModel model64 = this.Skill_skill;
            Skill_greedyskillModel model65 = this.Skill_greedyskill;
            Shop_GoldModel model66 = this.Shop_Gold;
            Skill_slotfirstModel model67 = this.Skill_slotfirst;
            Exp_expModel model68 = this.Exp_exp;
            Drop_GoldModel model69 = this.Drop_Gold;
            Drop_DropModel model70 = this.Drop_Drop;
            Stage_Level_activitylevelModel model71 = this.Stage_Level_activitylevel;
            Skill_slotoutModel model72 = this.Skill_slotout;
        }

        public static LocalModelManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LocalModelManager();
                }
                return _Instance;
            }
        }

        public Goods_foodModel Goods_food
        {
            get
            {
                if (this._Goods_foodModel == null)
                {
                    this._Goods_foodModel = new Goods_foodModel();
                }
                return this._Goods_foodModel;
            }
        }

        public Equip_equipModel Equip_equip
        {
            get
            {
                if (this._Equip_equipModel == null)
                {
                    this._Equip_equipModel = new Equip_equipModel();
                }
                return this._Equip_equipModel;
            }
        }

        public Skill_slotoutcostModel Skill_slotoutcost
        {
            get
            {
                if (this._Skill_slotoutcostModel == null)
                {
                    this._Skill_slotoutcostModel = new Skill_slotoutcostModel();
                }
                return this._Skill_slotoutcostModel;
            }
        }

        public Config_configModel Config_config
        {
            get
            {
                if (this._Config_configModel == null)
                {
                    this._Config_configModel = new Config_configModel();
                }
                return this._Config_configModel;
            }
        }

        public Box_SilverNormalBoxModel Box_SilverNormalBox
        {
            get
            {
                if (this._Box_SilverNormalBoxModel == null)
                {
                    this._Box_SilverNormalBoxModel = new Box_SilverNormalBoxModel();
                }
                return this._Box_SilverNormalBoxModel;
            }
        }

        public Skill_slotinModel Skill_slotin
        {
            get
            {
                if (this._Skill_slotinModel == null)
                {
                    this._Skill_slotinModel = new Skill_slotinModel();
                }
                return this._Skill_slotinModel;
            }
        }

        public Room_eventgameturnModel Room_eventgameturn
        {
            get
            {
                if (this._Room_eventgameturnModel == null)
                {
                    this._Room_eventgameturnModel = new Room_eventgameturnModel();
                }
                return this._Room_eventgameturnModel;
            }
        }

        public Operation_moveModel Operation_move
        {
            get
            {
                if (this._Operation_moveModel == null)
                {
                    this._Operation_moveModel = new Operation_moveModel();
                }
                return this._Operation_moveModel;
            }
        }

        public Character_BabyModel Character_Baby
        {
            get
            {
                if (this._Character_BabyModel == null)
                {
                    this._Character_BabyModel = new Character_BabyModel();
                }
                return this._Character_BabyModel;
            }
        }

        public Shop_itemModel Shop_item
        {
            get
            {
                if (this._Shop_itemModel == null)
                {
                    this._Shop_itemModel = new Shop_itemModel();
                }
                return this._Shop_itemModel;
            }
        }

        public Character_CharModel Character_Char
        {
            get
            {
                if (this._Character_CharModel == null)
                {
                    this._Character_CharModel = new Character_CharModel();
                }
                return this._Character_CharModel;
            }
        }

        public Language_lauguageModel Language_lauguage
        {
            get
            {
                if (this._Language_lauguageModel == null)
                {
                    this._Language_lauguageModel = new Language_lauguageModel();
                }
                return this._Language_lauguageModel;
            }
        }

        public Skill_dropinModel Skill_dropin
        {
            get
            {
                if (this._Skill_dropinModel == null)
                {
                    this._Skill_dropinModel = new Skill_dropinModel();
                }
                return this._Skill_dropinModel;
            }
        }

        public Drop_FakeDropModel Drop_FakeDrop
        {
            get
            {
                if (this._Drop_FakeDropModel == null)
                {
                    this._Drop_FakeDropModel = new Drop_FakeDropModel();
                }
                return this._Drop_FakeDropModel;
            }
        }

        public Room_eventangelskillModel Room_eventangelskill
        {
            get
            {
                if (this._Room_eventangelskillModel == null)
                {
                    this._Room_eventangelskillModel = new Room_eventangelskillModel();
                }
                return this._Room_eventangelskillModel;
            }
        }

        public Box_ChapterBoxModel Box_ChapterBox
        {
            get
            {
                if (this._Box_ChapterBoxModel == null)
                {
                    this._Box_ChapterBoxModel = new Box_ChapterBoxModel();
                }
                return this._Box_ChapterBoxModel;
            }
        }

        public Equip_UpgradeModel Equip_Upgrade
        {
            get
            {
                if (this._Equip_UpgradeModel == null)
                {
                    this._Equip_UpgradeModel = new Equip_UpgradeModel();
                }
                return this._Equip_UpgradeModel;
            }
        }

        public Stage_Level_activityModel Stage_Level_activity
        {
            get
            {
                if (this._Stage_Level_activityModel == null)
                {
                    this._Stage_Level_activityModel = new Stage_Level_activityModel();
                }
                return this._Stage_Level_activityModel;
            }
        }

        public Character_LevelModel Character_Level
        {
            get
            {
                if (this._Character_LevelModel == null)
                {
                    this._Character_LevelModel = new Character_LevelModel();
                }
                return this._Character_LevelModel;
            }
        }

        public Room_eventdemontext2loseModel Room_eventdemontext2lose
        {
            get
            {
                if (this._Room_eventdemontext2loseModel == null)
                {
                    this._Room_eventdemontext2loseModel = new Room_eventdemontext2loseModel();
                }
                return this._Room_eventdemontext2loseModel;
            }
        }

        public Achieve_AchieveModel Achieve_Achieve
        {
            get
            {
                if (this._Achieve_AchieveModel == null)
                {
                    this._Achieve_AchieveModel = new Achieve_AchieveModel();
                }
                return this._Achieve_AchieveModel;
            }
        }

        public Sound_soundModel Sound_sound
        {
            get
            {
                if (this._Sound_soundModel == null)
                {
                    this._Sound_soundModel = new Sound_soundModel();
                }
                return this._Sound_soundModel;
            }
        }

        public Box_ActivityModel Box_Activity
        {
            get
            {
                if (this._Box_ActivityModel == null)
                {
                    this._Box_ActivityModel = new Box_ActivityModel();
                }
                return this._Box_ActivityModel;
            }
        }

        public Fx_fxModel Fx_fx
        {
            get
            {
                if (this._Fx_fxModel == null)
                {
                    this._Fx_fxModel = new Fx_fxModel();
                }
                return this._Fx_fxModel;
            }
        }

        public Shop_MysticShopModel Shop_MysticShop
        {
            get
            {
                if (this._Shop_MysticShopModel == null)
                {
                    this._Shop_MysticShopModel = new Shop_MysticShopModel();
                }
                return this._Shop_MysticShopModel;
            }
        }

        public Curve_curveModel Curve_curve
        {
            get
            {
                if (this._Curve_curveModel == null)
                {
                    this._Curve_curveModel = new Curve_curveModel();
                }
                return this._Curve_curveModel;
            }
        }

        public Equip2_equip2Model Equip2_equip2
        {
            get
            {
                if (this._Equip2_equip2Model == null)
                {
                    this._Equip2_equip2Model = new Equip2_equip2Model();
                }
                return this._Equip2_equip2Model;
            }
        }

        public Stage_Level_chapter12Model Stage_Level_chapter12
        {
            get
            {
                if (this._Stage_Level_chapter12Model == null)
                {
                    this._Stage_Level_chapter12Model = new Stage_Level_chapter12Model();
                }
                return this._Stage_Level_chapter12Model;
            }
        }

        public Stage_Level_chapter13Model Stage_Level_chapter13
        {
            get
            {
                if (this._Stage_Level_chapter13Model == null)
                {
                    this._Stage_Level_chapter13Model = new Stage_Level_chapter13Model();
                }
                return this._Stage_Level_chapter13Model;
            }
        }

        public Stage_Level_chapter10Model Stage_Level_chapter10
        {
            get
            {
                if (this._Stage_Level_chapter10Model == null)
                {
                    this._Stage_Level_chapter10Model = new Stage_Level_chapter10Model();
                }
                return this._Stage_Level_chapter10Model;
            }
        }

        public Stage_Level_chapter11Model Stage_Level_chapter11
        {
            get
            {
                if (this._Stage_Level_chapter11Model == null)
                {
                    this._Stage_Level_chapter11Model = new Stage_Level_chapter11Model();
                }
                return this._Stage_Level_chapter11Model;
            }
        }

        public Buff_aloneModel Buff_alone
        {
            get
            {
                if (this._Buff_aloneModel == null)
                {
                    this._Buff_aloneModel = new Buff_aloneModel();
                }
                return this._Buff_aloneModel;
            }
        }

        public Goods_goodsModel Goods_goods
        {
            get
            {
                if (this._Goods_goodsModel == null)
                {
                    this._Goods_goodsModel = new Goods_goodsModel();
                }
                return this._Goods_goodsModel;
            }
        }

        public Box_SilverBoxModel Box_SilverBox
        {
            get
            {
                if (this._Box_SilverBoxModel == null)
                {
                    this._Box_SilverBoxModel = new Box_SilverBoxModel();
                }
                return this._Box_SilverBoxModel;
            }
        }

        public Room_soldierupModel Room_soldierup
        {
            get
            {
                if (this._Room_soldierupModel == null)
                {
                    this._Room_soldierupModel = new Room_soldierupModel();
                }
                return this._Room_soldierupModel;
            }
        }

        public Stage_Level_chapter9Model Stage_Level_chapter9
        {
            get
            {
                if (this._Stage_Level_chapter9Model == null)
                {
                    this._Stage_Level_chapter9Model = new Stage_Level_chapter9Model();
                }
                return this._Stage_Level_chapter9Model;
            }
        }

        public Test_AttrValueModel Test_AttrValue
        {
            get
            {
                if (this._Test_AttrValueModel == null)
                {
                    this._Test_AttrValueModel = new Test_AttrValueModel();
                }
                return this._Test_AttrValueModel;
            }
        }

        public Stage_Level_chapter7Model Stage_Level_chapter7
        {
            get
            {
                if (this._Stage_Level_chapter7Model == null)
                {
                    this._Stage_Level_chapter7Model = new Stage_Level_chapter7Model();
                }
                return this._Stage_Level_chapter7Model;
            }
        }

        public Stage_Level_chapter8Model Stage_Level_chapter8
        {
            get
            {
                if (this._Stage_Level_chapter8Model == null)
                {
                    this._Stage_Level_chapter8Model = new Stage_Level_chapter8Model();
                }
                return this._Stage_Level_chapter8Model;
            }
        }

        public Stage_Level_chapter5Model Stage_Level_chapter5
        {
            get
            {
                if (this._Stage_Level_chapter5Model == null)
                {
                    this._Stage_Level_chapter5Model = new Stage_Level_chapter5Model();
                }
                return this._Stage_Level_chapter5Model;
            }
        }

        public Stage_Level_chapter6Model Stage_Level_chapter6
        {
            get
            {
                if (this._Stage_Level_chapter6Model == null)
                {
                    this._Stage_Level_chapter6Model = new Stage_Level_chapter6Model();
                }
                return this._Stage_Level_chapter6Model;
            }
        }

        public Stage_Level_chapter3Model Stage_Level_chapter3
        {
            get
            {
                if (this._Stage_Level_chapter3Model == null)
                {
                    this._Stage_Level_chapter3Model = new Stage_Level_chapter3Model();
                }
                return this._Stage_Level_chapter3Model;
            }
        }

        public Room_roomModel Room_room
        {
            get
            {
                if (this._Room_roomModel == null)
                {
                    this._Room_roomModel = new Room_roomModel();
                }
                return this._Room_roomModel;
            }
        }

        public Room_eventdemontext2skillModel Room_eventdemontext2skill
        {
            get
            {
                if (this._Room_eventdemontext2skillModel == null)
                {
                    this._Room_eventdemontext2skillModel = new Room_eventdemontext2skillModel();
                }
                return this._Room_eventdemontext2skillModel;
            }
        }

        public Stage_Level_chapter4Model Stage_Level_chapter4
        {
            get
            {
                if (this._Stage_Level_chapter4Model == null)
                {
                    this._Stage_Level_chapter4Model = new Stage_Level_chapter4Model();
                }
                return this._Stage_Level_chapter4Model;
            }
        }

        public Stage_Level_chapter1Model Stage_Level_chapter1
        {
            get
            {
                if (this._Stage_Level_chapter1Model == null)
                {
                    this._Stage_Level_chapter1Model = new Stage_Level_chapter1Model();
                }
                return this._Stage_Level_chapter1Model;
            }
        }

        public Room_colorstyleModel Room_colorstyle
        {
            get
            {
                if (this._Room_colorstyleModel == null)
                {
                    this._Room_colorstyleModel = new Room_colorstyleModel();
                }
                return this._Room_colorstyleModel;
            }
        }

        public Stage_Level_chapter2Model Stage_Level_chapter2
        {
            get
            {
                if (this._Stage_Level_chapter2Model == null)
                {
                    this._Stage_Level_chapter2Model = new Stage_Level_chapter2Model();
                }
                return this._Stage_Level_chapter2Model;
            }
        }

        public Stage_Level_stagechapterModel Stage_Level_stagechapter
        {
            get
            {
                if (this._Stage_Level_stagechapterModel == null)
                {
                    this._Stage_Level_stagechapterModel = new Stage_Level_stagechapterModel();
                }
                return this._Stage_Level_stagechapterModel;
            }
        }

        public Shop_MysticShopShowModel Shop_MysticShopShow
        {
            get
            {
                if (this._Shop_MysticShopShowModel == null)
                {
                    this._Shop_MysticShopShowModel = new Shop_MysticShopShowModel();
                }
                return this._Shop_MysticShopShowModel;
            }
        }

        public Drop_harvestModel Drop_harvest
        {
            get
            {
                if (this._Drop_harvestModel == null)
                {
                    this._Drop_harvestModel = new Drop_harvestModel();
                }
                return this._Drop_harvestModel;
            }
        }

        public Box_TimeBoxModel Box_TimeBox
        {
            get
            {
                if (this._Box_TimeBoxModel == null)
                {
                    this._Box_TimeBoxModel = new Box_TimeBoxModel();
                }
                return this._Box_TimeBoxModel;
            }
        }

        public Preload_loadModel Preload_load
        {
            get
            {
                if (this._Preload_loadModel == null)
                {
                    this._Preload_loadModel = new Preload_loadModel();
                }
                return this._Preload_loadModel;
            }
        }

        public Room_levelModel Room_level
        {
            get
            {
                if (this._Room_levelModel == null)
                {
                    this._Room_levelModel = new Room_levelModel();
                }
                return this._Room_levelModel;
            }
        }

        public Shop_ReadyShopModel Shop_ReadyShop
        {
            get
            {
                if (this._Shop_ReadyShopModel == null)
                {
                    this._Shop_ReadyShopModel = new Shop_ReadyShopModel();
                }
                return this._Shop_ReadyShopModel;
            }
        }

        public Skill_superModel Skill_super
        {
            get
            {
                if (this._Skill_superModel == null)
                {
                    this._Skill_superModel = new Skill_superModel();
                }
                return this._Skill_superModel;
            }
        }

        public Shop_ShopModel Shop_Shop
        {
            get
            {
                if (this._Shop_ShopModel == null)
                {
                    this._Shop_ShopModel = new Shop_ShopModel();
                }
                return this._Shop_ShopModel;
            }
        }

        public Soldier_standardModel Soldier_standard
        {
            get
            {
                if (this._Soldier_standardModel == null)
                {
                    this._Soldier_standardModel = new Soldier_standardModel();
                }
                return this._Soldier_standardModel;
            }
        }

        public Skill_aloneModel Skill_alone
        {
            get
            {
                if (this._Skill_aloneModel == null)
                {
                    this._Skill_aloneModel = new Skill_aloneModel();
                }
                return this._Skill_aloneModel;
            }
        }

        public Goods_waterModel Goods_water
        {
            get
            {
                if (this._Goods_waterModel == null)
                {
                    this._Goods_waterModel = new Goods_waterModel();
                }
                return this._Goods_waterModel;
            }
        }

        public Soldier_soldierModel Soldier_soldier
        {
            get
            {
                if (this._Soldier_soldierModel == null)
                {
                    this._Soldier_soldierModel = new Soldier_soldierModel();
                }
                return this._Soldier_soldierModel;
            }
        }

        public Beat_beatModel Beat_beat
        {
            get
            {
                if (this._Beat_beatModel == null)
                {
                    this._Beat_beatModel = new Beat_beatModel();
                }
                return this._Beat_beatModel;
            }
        }

        public Weapon_weaponModel Weapon_weapon
        {
            get
            {
                if (this._Weapon_weaponModel == null)
                {
                    this._Weapon_weaponModel = new Weapon_weaponModel();
                }
                return this._Weapon_weaponModel;
            }
        }

        public Skill_skillModel Skill_skill
        {
            get
            {
                if (this._Skill_skillModel == null)
                {
                    this._Skill_skillModel = new Skill_skillModel();
                }
                return this._Skill_skillModel;
            }
        }

        public Skill_greedyskillModel Skill_greedyskill
        {
            get
            {
                if (this._Skill_greedyskillModel == null)
                {
                    this._Skill_greedyskillModel = new Skill_greedyskillModel();
                }
                return this._Skill_greedyskillModel;
            }
        }

        public Shop_GoldModel Shop_Gold
        {
            get
            {
                if (this._Shop_GoldModel == null)
                {
                    this._Shop_GoldModel = new Shop_GoldModel();
                }
                return this._Shop_GoldModel;
            }
        }

        public Skill_slotfirstModel Skill_slotfirst
        {
            get
            {
                if (this._Skill_slotfirstModel == null)
                {
                    this._Skill_slotfirstModel = new Skill_slotfirstModel();
                }
                return this._Skill_slotfirstModel;
            }
        }

        public Exp_expModel Exp_exp
        {
            get
            {
                if (this._Exp_expModel == null)
                {
                    this._Exp_expModel = new Exp_expModel();
                }
                return this._Exp_expModel;
            }
        }

        public Drop_GoldModel Drop_Gold
        {
            get
            {
                if (this._Drop_GoldModel == null)
                {
                    this._Drop_GoldModel = new Drop_GoldModel();
                }
                return this._Drop_GoldModel;
            }
        }

        public Drop_DropModel Drop_Drop
        {
            get
            {
                if (this._Drop_DropModel == null)
                {
                    this._Drop_DropModel = new Drop_DropModel();
                }
                return this._Drop_DropModel;
            }
        }

        public Stage_Level_activitylevelModel Stage_Level_activitylevel
        {
            get
            {
                if (this._Stage_Level_activitylevelModel == null)
                {
                    this._Stage_Level_activitylevelModel = new Stage_Level_activitylevelModel();
                }
                return this._Stage_Level_activitylevelModel;
            }
        }

        public Skill_slotoutModel Skill_slotout
        {
            get
            {
                if (this._Skill_slotoutModel == null)
                {
                    this._Skill_slotoutModel = new Skill_slotoutModel();
                }
                return this._Skill_slotoutModel;
            }
        }
    }
}

