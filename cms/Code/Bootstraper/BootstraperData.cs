namespace cms.Code.Bootstraper
{
	public class BootstraperData
	{
		public string black = "green";
		public string grayDarker = "#222";
		public string grayDark = "#333";
		public string gray = "#555";
		public string grayLight = "#999";
		public string grayLighter = "#eee";
		public string white = "#fff";

		// Accent colors
		// -------------------------
		public string blue=                  "#049cdb";
		public string blueDark=              "#0064cd";
		public string green=                 "#46a546";
		public string red=                   "#9d261d";
		public string yellow=                "#ffc40d";
		public string orange=                "#f89406";
		public string pink=                  "#c3325f";
		public string purple=                "#7a43b6";

			
		// Scaffolding
		// -------------------------
		public string bodyBackground = "@black";
		public string textColor = "@grayDark";


		// Links
		// -------------------------
		public string linkColor = "#08c";
		public string linkColorHover = "darken(linkColor, 15%)";


		// Typography
		// -------------------------
		public string sansFontFamily = "'Helvetica Neue', Helvetica, Arial, sans-serif";
		public string serifFontFamily = "Georgia, 'Times New Roman', Times, serif";
		public string monoFontFamily = "Menlo, Monaco, Consolas, 'Courier New', monospace";

		public string baseFontSize = "13px";
		public string baseFontFamily = "@sansFontFamily";
		public string baseLineHeight = "18px";
		public string altFontFamily = "@serifFontFamily";

		public string headingsFontFamily=    "inherit"; // empty to use BS default, baseFontFamily
		public string headingsFontWeight=    "bold";    // instead of browser default, bold
		public string headingsColor = "inherit"; // empty to use BS default, textColor


		// Tables
		// -------------------------
		public string tableBackground=                   "transparent"; // overall background-color
		public string tableBackgroundAccent=             "#f9f9f9"; // for striping
		public string tableBackgroundHover=              "#f5f5f5"; // for hover
		public string tableBorder=                       "#ddd"; // table and cell border

		// Buttons
		// -------------------------
		public string btnBackground=                     "@white";
		public string btnBackgroundHighlight=            "darken(white, 10%)";
		public string btnBorder=                         "#ccc";

		public string btnPrimaryBackground=              "@linkColor";
		public string btnPrimaryBackgroundHighlight=     "spin(@btnPrimaryBackground, 15%)";

		public string btnInfoBackground=                 "#5bc0de";
		public string btnInfoBackgroundHighlight=        "#2f96b4";

		public string btnSuccessBackground=              "#62c462";
		public string btnSuccessBackgroundHighlight=     "#51a351";

		public string btnWarningBackground=              "lighten(orange, 15%)";
		public string btnWarningBackgroundHighlight=     "@orange";

		public string btnDangerBackground=               "#ee5f5b";
		public string btnDangerBackgroundHighlight=      "#bd362f";

		public string btnInverseBackground=              "@gray";
		public string btnInverseBackgroundHighlight=     "@grayDarker";


		// Forms
		// -------------------------
		public string inputBackground=               "@white";
		public string inputBorder=                   "#ccc";
		public string inputBorderRadius=             "3px";
		public string inputDisabledBackground=       "@grayLighter";
		public string formActionsBackground=         "#f5f5f5";

		// Dropdowns
		// -------------------------
		public string dropdownBackground=            "@white";
		public string dropdownBorder=                "rgba(0,0,0,.2)";
		public string dropdownLinkColor=             "@grayDark";
		public string dropdownLinkColorHover=        "@white";
		public string dropdownLinkBackgroundHover=   "@linkColor";

		// COMPONENT VARIABLES
		// --------------------------------------------------

		// Z-index master list
		// -------------------------
		// Used for a bird's eye view of components dependent on the z-axis
		// Try to avoid customizing these =)
		public int zindexDropdown = 1000;
		public int zindexPopover = 1010;
		public int zindexTooltip = 1020;
		public int zindexFixedNavbar = 1030;
		public int zindexModalBackdrop = 1040;
		public int zindexModal = 1050;
		// Sprite icons path
		// -------------------------
		public string iconSpritePath=          "../img/glyphicons-halflings.png";
		public string iconWhiteSpritePath=     "../img/glyphicons-halflings-white.png";
		// Input placeholder text color
		// -------------------------
		public string placeholderText=         "@grayLight";

		// Hr border color
		// -------------------------
		public string hrBorder=                "@grayLighter";

		// Navbar
		// -------------------------
		public string navbarHeight=                    "40px";
		public string navbarBackground=                "@grayDarker";
		public string navbarBackgroundHighlight=       "@grayDark";

		public string navbarText=                      "@grayLight";
		public string navbarLinkColor=                 "@grayLight";
		public string navbarLinkColorHover=            "@white";
		public string navbarLinkColorActive=           "@navbarLinkColorHover";
		public string navbarLinkBackgroundHover=       "transparent";
		public string navbarLinkBackgroundActive=      "@navbarBackground";

		public string navbarSearchBackground=          "lighten(@navbarBackground, 25%)";
		public string navbarSearchBackgroundFocus=     "@white";
		public string navbarSearchBorder=              "darken(@navbarSearchBackground, 30%)";
		public string navbarSearchPlaceholderColor=    "#ccc";
		public string navbarBrandColor=                "@navbarLinkColor";

		// Hero unit
		// -------------------------
		public string heroUnitBackground=              "@grayLighter";
		public string heroUnitHeadingColor=            "inherit";
		public string heroUnitLeadColor=               "inherit";


		// Form states and alerts
		// -------------------------
		public string warningText=             "#c09853";
		public string warningBackground=       "#fcf8e3";
		public string warningBorder=           "darken(spin(@warningBackground, -10), 3%)";

		public string errorText=               "#b94a48";
		public string errorBackground=         "#f2dede";
		public string errorBorder=             "darken(spin(@errorBackground, -10), 3%)";

		public string successText=             "#468847";
		public string successBackground=       "#dff0d8";
		public string successBorder=           "darken(spin(@successBackground, -10), 5%)";

		public string infoText=                "#3a87ad";
		public string infoBackground=          "#d9edf7";
		public string infoBorder=              "darken(spin(@infoBackground, -10), 7%)";

		// GRID
		// --------------------------------------------------

		// Default 940px grid
		// -------------------------
		public int gridColumns = 12;
		public string gridColumnWidth=         "60px";
		public string gridGutterWidth=         "20px";
		public string gridRowWidth = "(@gridColumns * @gridColumnWidth) + (@gridGutterWidth * (@gridColumns - 1))";

		// Fluid grid
		// -------------------------
		public string fluidGridColumnWidth=    "6.382978723%";
		public string fluidGridGutterWidth = "2.127659574%";


	}
}




/*
// Grays
// -------------------------
	@black:                 green;
	@grayDarker:            #222;
	@grayDark:              #333;
	@gray:                  #555;
	@grayLight:             #999;
	@grayLighter:           #eee;
	@white:                 #fff;


	// Accent colors
	// -------------------------
	@blue:                  #049cdb;
	@blueDark:              #0064cd;
	@green:                 #46a546;
	@red:                   #9d261d;
	@yellow:                #ffc40d;
	@orange:                #f89406;
	@pink:                  #c3325f;
	@purple:                #7a43b6;


	// Scaffolding
	// -------------------------
	@bodyBackground:        @black;
	@textColor:             @grayDark;


	// Links
	// -------------------------
	@linkColor:             #08c;
	@linkColorHover:        darken(@linkColor, 15%);


	// Typography
	// -------------------------
	@sansFontFamily:        "Helvetica Neue", Helvetica, Arial, sans-serif;
	@serifFontFamily:       Georgia, "Times New Roman", Times, serif;
	@monoFontFamily:        Menlo, Monaco, Consolas, "Courier New", monospace;

	@baseFontSize:          13px;
	@baseFontFamily:        @sansFontFamily;
	@baseLineHeight:18px;
	@altFontFamily:         @serifFontFamily;

	@headingsFontFamily:    inherit; // empty to use BS default, @baseFontFamily
	@headingsFontWeight:    bold;    // instead of browser default, bold
	@headingsColor:         inherit; // empty to use BS default, @textColor


	// Tables
	// -------------------------
	@tableBackground:                   transparent; // overall background-color
	@tableBackgroundAccent:             #f9f9f9; // for striping
	@tableBackgroundHover:              #f5f5f5; // for hover
	@tableBorder:                       #ddd; // table and cell border


	// Buttons
	// -------------------------
	@btnBackground:                     @white;
	@btnBackgroundHighlight:            darken(@white, 10%);
	@btnBorder:                         #ccc;

	@btnPrimaryBackground:              @linkColor;
	@btnPrimaryBackgroundHighlight:     spin(@btnPrimaryBackground, 15%);

	@btnInfoBackground:                 #5bc0de;
	@btnInfoBackgroundHighlight:        #2f96b4;

	@btnSuccessBackground:              #62c462;
	@btnSuccessBackgroundHighlight:     #51a351;

	@btnWarningBackground:              lighten(@orange, 15%);
	@btnWarningBackgroundHighlight:     @orange;

	@btnDangerBackground:               #ee5f5b;
	@btnDangerBackgroundHighlight:      #bd362f;

	@btnInverseBackground:              @gray;
	@btnInverseBackgroundHighlight:     @grayDarker;


	// Forms
	// -------------------------
	@inputBackground:               @white;
	@inputBorder:                   #ccc;
	@inputBorderRadius:             3px;
	@inputDisabledBackground:       @grayLighter;
	@formActionsBackground:         #f5f5f5;

	// Dropdowns
	// -------------------------
	@dropdownBackground:            @white;
	@dropdownBorder:                rgba(0,0,0,.2);
	@dropdownLinkColor:             @grayDark;
	@dropdownLinkColorHover:        @white;
	@dropdownLinkBackgroundHover:   @linkColor;




	// COMPONENT VARIABLES
	// --------------------------------------------------

	// Z-index master list
	// -------------------------
	// Used for a bird's eye view of components dependent on the z-axis
	// Try to avoid customizing these :)
	@zindexDropdown:          1000;
	@zindexPopover:           1010;
	@zindexTooltip:           1020;
	@zindexFixedNavbar:       1030;
	@zindexModalBackdrop:     1040;
	@zindexModal:             1050;


	// Sprite icons path
	// -------------------------
	@iconSpritePath:          "../img/glyphicons-halflings.png";
	@iconWhiteSpritePath:     "../img/glyphicons-halflings-white.png";


	// Input placeholder text color
	// -------------------------
	@placeholderText:         @grayLight;


	// Hr border color
	// -------------------------
	@hrBorder:                @grayLighter;


	// Navbar
	// -------------------------
	@navbarHeight:                    40px;
	@navbarBackground:                @grayDarker;
	@navbarBackgroundHighlight:       @grayDark;

	@navbarText:                      @grayLight;
	@navbarLinkColor:                 @grayLight;
	@navbarLinkColorHover:            @white;
	@navbarLinkColorActive:           @navbarLinkColorHover;
	@navbarLinkBackgroundHover:       transparent;
	@navbarLinkBackgroundActive:      @navbarBackground;

	@navbarSearchBackground:          lighten(@navbarBackground, 25%);
	@navbarSearchBackgroundFocus:     @white;
	@navbarSearchBorder:              darken(@navbarSearchBackground, 30%);
	@navbarSearchPlaceholderColor:    #ccc;
	@navbarBrandColor:                @navbarLinkColor;


	// Hero unit
	// -------------------------
	@heroUnitBackground:              @grayLighter;
	@heroUnitHeadingColor:            inherit;
	@heroUnitLeadColor:               inherit;


	// Form states and alerts
	// -------------------------
	@warningText:             #c09853;
	@warningBackground:       #fcf8e3;
	@warningBorder:           darken(spin(@warningBackground, -10), 3%);

	@errorText:               #b94a48;
	@errorBackground:         #f2dede;
	@errorBorder:             darken(spin(@errorBackground, -10), 3%);

	@successText:             #468847;
	@successBackground:       #dff0d8;
	@successBorder:           darken(spin(@successBackground, -10), 5%);

	@infoText:                #3a87ad;
	@infoBackground:          #d9edf7;
	@infoBorder:              darken(spin(@infoBackground, -10), 7%);



	// GRID
	// --------------------------------------------------

	// Default 940px grid
	// -------------------------
	@gridColumns:             12;
	@gridColumnWidth:         60px;
	@gridGutterWidth:         20px;
	@gridRowWidth:            (@gridColumns * @gridColumnWidth) + (@gridGutterWidth * (@gridColumns - 1));

	// Fluid grid
	// -------------------------
	@fluidGridColumnWidth:    6.382978723%;
	@fluidGridGutterWidth:    2.127659574%;

*/
