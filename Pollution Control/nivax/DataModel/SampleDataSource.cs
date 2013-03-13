using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Facts About Pollution",
                    "Facts About Pollution",
                    "Assets/Images/10.jpg",
                    "A heart attack occurs when blood flow to a part of your heart is blocked for a long enough time that part of the heart muscle is damaged or dies. The medical term for this is myocardial infarction.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Health risks",
                    "Have you looked around lately and noticed that various types of pollution take place all around us every single day?",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nHave you looked around lately and noticed that various types of pollution take place all around us every single day? Air pollution, water pollution, land pollution and all sorts of pollution proliferate.Pollution damages our environment because it releases various types of harmful particles into the air. When we breathe them in, they can lower our overall resistance level and put our health at enormous risk. The long term effects include global warming due to the ozone layer being damaged.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Facts About Pollution", CreatedOnTwo = "Item", CreatedTxtTwo = "Health Risks", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Pollution Control" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Water pollution",
                     "While it could be that we are usually more exposed to the effects of air pollution, it isn't the only type that we should be concerned with.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWhile it could be that we are usually more exposed to the effects of air pollution, it isn't the only type that we should be concerned with. We should also be wary about water pollution as it can destroy the aquatic life and mess up the balance of the food chain. Contentious issues such as oil being spilled in the water has really created long term problems. It is illegal to dump chemicals and waste into the lakes but it has also created serious issues because many still engage in such behaviors. And how about the open sewer system common to most (all?) vessels and boats that sail daily through the vast expanse of oceans and seas? Would you think they would ever care to carry heavy human wastes while cruising? As one natural doctor I once heard telling her patients: The ocean is one big toilet. As much as possible, her advice was for people to eat cultured fish, instead.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Facts About Pollution", CreatedOnTwo = "Item", CreatedTxtTwo = "Water Pollution", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Pollution Control" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                    "Land pollution",
                    "The soil can become polluted as well and that is dangerous. We depend on the soil to offer us a place to grow food.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nThe soil can become polluted as well and that is dangerous. We depend on the soil to offer us a place to grow food. It is also what allows trees, plants, and flowers to grow. Without the balance of  nature in place,  the quality of air we breathe  will suffer.  If people will not care at all and worse, it they become careless, this will become a cycle that can lead to long term problems in our society  that could be irreversible.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Facts About Pollution", CreatedOnTwo = "Item", CreatedTxtTwo = "Land Pollution", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Pollution Control" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Better pollution awareness",
                     "To a great extent, industrial development brings with it so much pollution.  But since  majority of the world’s population  would naturally not be willing to give up the luxuries they enjoy.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTo a great extent, industrial development brings with it so much pollution.  But since  majority of the world’s population  would naturally not be willing to give up the luxuries they enjoy, the option is to look for cleaner methods of getting what we want. This includes changing fuel sources and cutting back on the amount of pollution that we personally make. Individuals can carpool, pick up trash, be more careful about the product they buy, and continue to educate others about pollution. The right education and awareness place a huge factor when it comes to reducing the levels of pollution. In some cases the government has had to directly intervene through the issuance of  regulations so that  our rivers and the air can stay as clean as possible.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Facts About Pollution", CreatedOnTwo = "Item", CreatedTxtTwo = "Better Pollution Awareness", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Pollution Control" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                    "Penalties and incentives",
                    "There are serious fines and even jail penalties associated with not following government guidelines.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nThere are serious fines and even jail penalties associated with not following government guidelines. In other instances the government offers nice incentives for those that do and exert ordinary or special but meaningful efforts to minimize  pollution voluntarily. These could be  in the form of cash incentives offered to both households and businesses. Incentives may truly help, but, even without them, we should all be responsible citizens of this world.  Hand in hand, let us  keep our environment pollution-free.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Facts About Pollution", CreatedOnTwo = "Item", CreatedTxtTwo = "Panelities and Incentives", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Pollution Control" });
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                   "Ways on How to Prevent, Reduce",
                   "Ways on How to Prevent, Reduce",
                   "Assets/Images/20.jpg",
                   "Pollution prevention is an exceptionally major global concern because of the harmful effects of pollution on the person’s health and on the environment. Environmental pollution comes in various forms, such as: air pollution, water pollution, soil pollution, etc..");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    " Environmental protection",
                    "An echocardiogram is a test that uses sound waves to create a moving picture of the heart. The picture is much more detailed than a plain x-ray image and involves no radiation exposure.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nEveryone is a stakeholder as we are all inhabitants of this one and only mother earth. Each person has something to contribute to advance an effective pollution prevention awareness initiatives. Environmental protection is caring for ourselves, loving our children and ensuring a sustainable future for generations to come. 'If we heal the earth, we heal ourselves.' You and I should therefore accept personal responsibility for the success of the environmental protection programs of our respective community by cooperating and actively participating in making the atmosphere pollution free. Help stop pollution today. Although on an individual basis we can help combat pollution in our own immediate environment, efficient control can be best institutionalized through legislation. Thus, most countries have already addressed the issue by passing some form of pollution prevention measures. Averting the onset of pollution in any area, i.e. be it on air, water or land, could be the start and the simplest preventive solution to the problem. This calls for a conscientious effort to adopt good practices or habits by the people, the passage and the proper implementation of appropriate government laws and the strict compliance especially by potential industrial pollutants. If there are no pollutants, there will be no pollution. And yet, this is easier said than done. Certain bad habits are entrenched and industrial development somehow carries with it the concomitant burden of pollution. The cost to business and its commercial ramifications make this rather simple preventive approach quite complicated and more difficult to implement.",
                    group2) { CreatedOn = "Group", CreatedTxt = "Ways on How to Prevent, Reduce", CreatedOnTwo = "Item", CreatedTxtTwo = "Environmental Protection", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/21.jpg")), CurrentStatus = "Pollution Control" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                     "How can we help?",
                     "An exercise stress test is a screening tool used to test the effect of exercise on your heart.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe good news is that there is hope. This seemingly difficult situation does not deter environmental protection advocates to pursue their dream for a more pollution-free earth. Kudos to Greenpeace and all similar organizations all over the world as they bear for us the campaign torch on environmental issues. Everyone can help by self education and by adopting good and healthy practices. It is also important that we help raise awareness about the significance of environmental issues, their dire consequences and what can be done. 'One person alone cannot save the planet’s biodiversity, but each individual’s effort to encourage nature’s wealth must not be underestimated.'- United Nations Environment Programme (UNEP)\n\nEvery action or inaction of any person in regard to her or his surroundings has an effect- be it good, neutral or bad- on the environment. Nature already provides for our needs.Whatever we do to it gets back to us. If we are friends of the earth, it will also be friendly to us. By becoming aware and doing the right action, we choose to be part of the solution. What comes to mind now to serve as reminders include the following:\n\nStop smoking or at least follow the 'No Smoking' sign.\n\nUse unleaded gasoline in your cars.\n\nKeep your car properly maintained to keep it in good running condition to avoid smoke emissions.\n\nShare a ride or engage in car pooling.\n\nInstead of using your cars, choose to walk or ride a bicycle whenever possible. With this eco-friendly practice, you will also be healthier and happier by staying fit.\n\nNever use open fires to dispose of wastes.\n\nAdopt the 3Rs of solid waste management: reduce, reuse and recycle. Inorganic materials such as metals, glass and plastic; also organic materials like paper, can be reclaimed and recycled. This takes into account that the proven solution to the problem of proper waste management (especially in third world countries) is proper disposal (in waste bins for collection and not in the street where it could fall into drains), waste segregation and collection, and recycling.\n\nStart composting brown leaves in your yard and green scraps from your kitchen. It will reduce waste while improving your yard and garden soils.\n\nReconnect with nature. Live green by using green power supplied abundantly and freely by wind and the sun. Hang your laundry to dry to minimize use of gas or electricity from your dryers. Enjoy fresh air from open windows to lessen the use of air conditioning system.\n\nPatronize local foods and goods. In this manner, transporting goods and foods prepared with GMOs which uses fuel from conventional energy sources will be minimized.\n\nUse eco-friendly or biodegradable materials instead of plastic which are made up of highly toxic substances injurious to your health.\n\nCreate your green space. Value your garden. Plant more trees and put indoor plants in your homes.They clean the air, provide oxygen and beautify your surrounding. Thus, care for them and by protecting them, especially the big trees around and in the forest, you protect yourself and your family, too.\n\nHave a proper waste disposal system especially for toxic wastes\n\nTake very good care of your pets and their wastes.\n\nNever throw, run or drain or dispose into the water, air, or land any substance in solid, liquid or gaseous form that shall cause pollution.\n\nDo not cause loud noises and unwanted sounds to avoid noise pollution.\n\nDo not litter in public places. Anti-litter campaigns can educate the populace.\n\nIndustries should use fuel with lower sulphur content.\n\nIndustries should monitor their air emissions regularly and take measures to ensure compliance with the prescribed emission standards.\n\nIndustries should strictly follow applicable government regulations on pollution control.\n\nOrganic waste should be dumped in places far from residential areas.\n\nSay a big 'NO' to GMOs or genetically modified organisms. Genetically engineered crops are not only bad for the environment since they require massive amount of fungicides, pesticides, and herbicides; but GMO altered foods are also health risks and negatively impact farmers' livelihood.\n\nHelp stop pollution. Join the Earth Day celebration every April 22nd and consider making it an everyday practice for the rest of your earthly life.",
                     group2) { CreatedOn = "Group", CreatedTxt = "How to Prevent", CreatedOnTwo = "Item", CreatedTxtTwo = "How can we help?", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/22.jpg")), CurrentStatus = "Pollution Control" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                      "Final thoughts",
                      "Thallium stress test is a nuclear imaging method that shows how well blood flows into the heart muscle, both at rest and during activity.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nLet me leave you with the following excerpts from Eckhart Tolle's 'The Power of Now'.\n\n. . . The pollution of the planet is only an outside reflection of an inner psychic pollution: millions of unconscious individuals not taking responsibility for their inner space. Are you polluting the world or cleaning up the mess? You are responsible for your inner space; nobody else is, just as you are responsible for the planet. As within, so without. If humans clear inner pollution, then they will also cease to create outer pollution.",
                      group2) { CreatedOn = "Group", CreatedTxt = "How to Prevent", CreatedOnTwo = "Item", CreatedTxtTwo = "Final Thoughts", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/23.jpg")), CurrentStatus = "Pollution Control" });
            this.AllGroups.Add(group2);
			
            var group3 = new SampleDataGroup("Group-3",
                   "Choices to Help the Environment",
                   "Choices to Help the Environment",
                   "Assets/Images/30.jpg",
                   "Economic activity often result in the negative effects of globalization, such as chemical waste and air pollution. Still, the cost of cleaning these effects up is not included in the price of a product. What can you do to live green? What can you do to help the environment and reduce your carbon footprint? There are so many choices and options available that the average person can do. ");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "Negative Effects of Globalization",
                    "Some economists studying globalization have come up with a theory that this behaviour follows a pattern first discovered by economist Simon Kuznets. According to Kuznets' theory income inequality keeps increasing with per capita income until a critical income level is reached, after which inequality declines again. The graphic representation of the Kuznets theory is called the Kuznets curve, and it resembles a bell curve.",
                    "Assets/DarkGray.png",
                    "",
                    "Negative Effects of Globalization - Chemical Waste and Air Pollution\n\nEconomic activity often result in the negative effects of globalization, such as chemical waste and air pollution. Still, the cost of cleaning these effects up is not included in the price of a product. In many cases, specifically in the developing world, manafacturers take advantage of lax regulations and cause air pollution and chemical waste without restraint. If Kuznets logic can also be applied to the relationship between income and environment, it results in a 'environmental' Kuznets curve, which shows the negative environmental effects rising with per capita income. But once a tipping point is reached, a higher per capita income start correlating with an improving environment. This can clearly be observed in developing nations where per capita income is low, and more pressing social needs enjoys priority over environmental regulation. For a while economic development means more negative environmental effects like air pollution and chemical waste, but as soon as a country becomes more developed, more regulations are set in place to protect the environment. First, environmental harm is stabilized, and as income rises further, some of the new wealth is used to reverse the damage already done.\n\nThis theory holds important implications for the debate about global warming, because it implies that greenhouse emissions from developing countries(especially China and India) will continue until a tipping point in per capita income is arrived at. It turns out that very little empirical evidence exists to prove this theory however. The relationship this theory describes has so far been shown to only apply to urban concentrations of sulfur dioxide. The environmental Kuznets theory rests on two assumptions, which is that environmentally friendly production is more expensive than environmentally unfriendly production, and that poor countries can't afford environmentally safe production until they reach a certain income level. These assumptions could be false however.\n\nDenmark's economy have recently grown by 50% without an increase in greenhouse emissions, thereby disproving the first assumption. Denmark achieved this by shifting 22% of its electricity generation to wind power. The success of ecolabeling, a market-orientated program that promotes environmentally friendly products, also suggests that economic growth does not always have to coincide with environmental harm.The environmental Kuznets theory states that economic growth in poor countries will always coincide with negative environmental effects until a certain income level is reached.",
                    group3) { CreatedOn = "Group", CreatedTxt = "Help the Environment", CreatedOnTwo = "Item", CreatedTxtTwo = "Negative Effects of Globalization", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/31.jpg")), CurrentStatus = "Pollution Control" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                     "How to Live Green",
                     "What can you do to live green? What can you do to help the environment and reduce your carbon footprint? There are so many choices and options available that the average person can do. These things don't have to cost a fortune and are easy to incorporate into daily living.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nSome of the most basic things you can do to live green involve using less. Turn your lights off and use natural light to use less electricity, turn your heat down to use less gas or oil, combine your errands so that you use less gasoline, cook multiple items at the same time so that you turn your oven on only once a day instead of four times, and turn the water off when you brush your teeth. Many people worry that living green will cost you money; however all of the above ideas will actually save you money. Another way to help the environment and reduce your carbon footprint is to reuse items. There are two ways to reuse items. Reusing them for their original purpose, such as ziploc bags, tinfoil, plastic utensils, etc and finding new uses for things you already own. By doing this you are not consuming any more of the world's precious resources. If you can't reuse something of your own, purchasing something secondhand instead of brand new is another way to reduce your consumption of resources. Recycling is a great way to reuse items. Many cities offer free recycling, all you need to do is start taking advantage of it. If recycling is not offered curbside there are other options. I know many organizations, such as churches, raise money by collecting paper products. You will notice these bins in many parking lots. They typically take newspaper and magazines, and sometimes boxes. This is a great way to help the organization raise some money, while recycling your papers instead of throwing them away. Do you use a lot of aluminum cans? You can make a little money by taking these to a can depositor. Check your phone book to see if there is a place near you.\n\nThere are also some options that will cost you a little bit of money - at least upfront. My grocery store sells cloth grocery bags for $.99 each. Yes I had to buy them to avoid using all those plastic bags the grocery store loves to give out, however I also get a nickel back for each bag of my own I bring in. Slowly I will make my money back from this investment and I am helping the environment in the process. Replacing your incandescent light bulbs with fluorescent bulbs will cost you upfront as well. I purchase these in bulk from Sams Club, eight of them for $11.97. My estimate is that I spent about $6 more to purchase these bulbs that use so much less energy and last so much longer. It won't take much time for me to make my money back in energy savings. And it is better for the environment than incandescent bulbs. Another huge thing a parent can do to live green is to use cloth diapers as opposed to disposables. Disposable diapers take up an enormous amount of this countries landfills, don't break down easily, cost a lot of money, put chemicals directly on the bottoms of our children, and in many cases delay potty training. Yes it is a chunk of change in the beginning, but a couple of hundred dollars spent upfront will save you thousands in the long run, especially if you use them on more than one child. Other easy things a person can do to live green include carpooling to work or the store, using the Diva Cup or Keeper instead of disposable sanitary products, bringing your own mug to coffee places instead of using their disposable cups, turning up the thermostat in the summer to reduce electricity usage of your AC, putting a sweater on instead of turning up the heat, borrowing books and magazines from the library instead of purchasing new (it takes 15-20 trees to make the paper for a new average sized adult book), using cloth napkins instead of paper, using rags instead of paper towels, cooking from scratch (uses less packaging than convenience foods), and getting your ink cartridges refilled instead of buying new.\n\nPurchasing as much as you can from local resources will help reduce fuel consumption and pollution. Why not eat the oranges from Florida instead of the bananas from another country? Better yet, purchase food items that are grown within the same state or county as you to really reduce fuel consumption. An added benefit of purchasing locally is that you are supporting people in your own community, state and country, as opposed to people halfway around the world. This could cost you a little more money depending on what is in season; however it doesn't automatically cost more to eat locally. In the summer and fall especially this should reduce your grocery bill. Composting and growing your own food are also ways to help the environment and live green. Doing things yourself instead of hiring a third party helps the environment as well. When you hire someone else to do something there is fuel use involved in transporting the person to you or transporting you to the person. Cutting your own hair or at least your kids saves a trip to the barber as well as saves you money and time. On the high end of living green you could always install solar panels on your home, invest in wind energy, or purchase a hybrid car. These cost a good bit of money, but if money isn't an object and you really want to help the environment these are great ways to do it. Living green does not have to be hard and it does not have to cost money. I find it very easy to do most of these things. Start small if you need to, but try to incorporate these things, and more, into your daily life. It takes some thinking about it in the beginning, but in time you won't even notice. By living green you will be helping the environment as well as saving money.",
                     group3) { CreatedOn = "Group", CreatedTxt = "Help the Environment", CreatedOnTwo = "Item", CreatedTxtTwo = "How to live Green?", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/32.jpg")), CurrentStatus = "Pollution Control" });
            this.AllGroups.Add(group3);
        }
    }
}
