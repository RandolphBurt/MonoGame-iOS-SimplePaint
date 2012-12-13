﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.17929.
// 
namespace Paint.ToolboxLayout {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class ToolboxLayoutDefinition {
        
        private BorderType borderField;
        
        private int maximizedHeightField;
        
        private int minimizedHeightField;
        
        private int widthField;
        
        private ColorType backgroundColorField;
        
        private ToolboxLayoutDefinitionStandardTools standardToolsField;
        
        private ToolboxLayoutDefinitionPaintTools paintToolsField;
        
        private ToolboxLayoutDefinitionPlaybackTools playbackToolsField;
        
        /// <remarks/>
        public BorderType Border {
            get {
                return this.borderField;
            }
            set {
                this.borderField = value;
            }
        }
        
        /// <remarks/>
        public int MaximizedHeight {
            get {
                return this.maximizedHeightField;
            }
            set {
                this.maximizedHeightField = value;
            }
        }
        
        /// <remarks/>
        public int MinimizedHeight {
            get {
                return this.minimizedHeightField;
            }
            set {
                this.minimizedHeightField = value;
            }
        }
        
        /// <remarks/>
        public int Width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <remarks/>
        public ColorType BackgroundColor {
            get {
                return this.backgroundColorField;
            }
            set {
                this.backgroundColorField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionStandardTools StandardTools {
            get {
                return this.standardToolsField;
            }
            set {
                this.standardToolsField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPaintTools PaintTools {
            get {
                return this.paintToolsField;
            }
            set {
                this.paintToolsField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPlaybackTools PlaybackTools {
            get {
                return this.playbackToolsField;
            }
            set {
                this.playbackToolsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BorderType {
        
        private int widthField;
        
        private ColorType colorField;
        
        /// <remarks/>
        public int Width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <remarks/>
        public ColorType Color {
            get {
                return this.colorField;
            }
            set {
                this.colorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ColorType {
        
        private byte redField;
        
        private byte greenField;
        
        private byte blueField;
        
        /// <remarks/>
        public byte Red {
            get {
                return this.redField;
            }
            set {
                this.redField = value;
            }
        }
        
        /// <remarks/>
        public byte Green {
            get {
                return this.greenField;
            }
            set {
                this.greenField = value;
            }
        }
        
        /// <remarks/>
        public byte Blue {
            get {
                return this.blueField;
            }
            set {
                this.blueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GaugeType {
        
        private int verticalMarginField;
        
        private int horizontalMarginField;
        
        private int widthField;
        
        private int markerWidthField;
        
        /// <remarks/>
        public int VerticalMargin {
            get {
                return this.verticalMarginField;
            }
            set {
                this.verticalMarginField = value;
            }
        }
        
        /// <remarks/>
        public int HorizontalMargin {
            get {
                return this.horizontalMarginField;
            }
            set {
                this.horizontalMarginField = value;
            }
        }
        
        /// <remarks/>
        public int Width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <remarks/>
        public int MarkerWidth {
            get {
                return this.markerWidthField;
            }
            set {
                this.markerWidthField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RegionType {
        
        private RegionTypeLocation locationField;
        
        private RegionTypeSize sizeField;
        
        private ColorType backgroundColorField;
        
        private BorderType borderField;
        
        /// <remarks/>
        public RegionTypeLocation Location {
            get {
                return this.locationField;
            }
            set {
                this.locationField = value;
            }
        }
        
        /// <remarks/>
        public RegionTypeSize Size {
            get {
                return this.sizeField;
            }
            set {
                this.sizeField = value;
            }
        }
        
        /// <remarks/>
        public ColorType BackgroundColor {
            get {
                return this.backgroundColorField;
            }
            set {
                this.backgroundColorField = value;
            }
        }
        
        /// <remarks/>
        public BorderType Border {
            get {
                return this.borderField;
            }
            set {
                this.borderField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class RegionTypeLocation {
        
        private float xField;
        
        private float yField;
        
        /// <remarks/>
        public float X {
            get {
                return this.xField;
            }
            set {
                this.xField = value;
            }
        }
        
        /// <remarks/>
        public float Y {
            get {
                return this.yField;
            }
            set {
                this.yField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class RegionTypeSize {
        
        private int widthField;
        
        private int heightField;
        
        /// <remarks/>
        public int Width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <remarks/>
        public int Height {
            get {
                return this.heightField;
            }
            set {
                this.heightField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionStandardTools {
        
        private ToolboxLayoutDefinitionStandardToolsButtons buttonsField;
        
        /// <remarks/>
        public ToolboxLayoutDefinitionStandardToolsButtons Buttons {
            get {
                return this.buttonsField;
            }
            set {
                this.buttonsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionStandardToolsButtons {
        
        private ToolboxLayoutDefinitionStandardToolsButtonsButton[] buttonField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Button")]
        public ToolboxLayoutDefinitionStandardToolsButtonsButton[] Button {
            get {
                return this.buttonField;
            }
            set {
                this.buttonField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionStandardToolsButtonsButton {
        
        private ToolboxLayoutDefinitionStandardToolsButtonsButtonButtonType buttonTypeField;
        
        private RegionType regionField;
        
        /// <remarks/>
        public ToolboxLayoutDefinitionStandardToolsButtonsButtonButtonType ButtonType {
            get {
                return this.buttonTypeField;
            }
            set {
                this.buttonTypeField = value;
            }
        }
        
        /// <remarks/>
        public RegionType Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum ToolboxLayoutDefinitionStandardToolsButtonsButtonButtonType {
        
        /// <remarks/>
        Exit,
        
        /// <remarks/>
        Undo,
        
        /// <remarks/>
        Redo,
        
        /// <remarks/>
        ToggleMaxMin,
        
        /// <remarks/>
        ToggleDock,
        
        /// <remarks/>
        PlayPausePlayback,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPaintTools {
        
        private ToolboxLayoutDefinitionPaintToolsColorSetter colorSetterField;
        
        private ToolboxLayoutDefinitionPaintToolsBrushSizeSelector brushSizeSelectorField;
        
        private ToolboxLayoutDefinitionPaintToolsColorSelector colorSelectorField;
        
        private ToolboxLayoutDefinitionPaintToolsColorPickers colorPickersField;
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPaintToolsColorSetter ColorSetter {
            get {
                return this.colorSetterField;
            }
            set {
                this.colorSetterField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPaintToolsBrushSizeSelector BrushSizeSelector {
            get {
                return this.brushSizeSelectorField;
            }
            set {
                this.brushSizeSelectorField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPaintToolsColorSelector ColorSelector {
            get {
                return this.colorSelectorField;
            }
            set {
                this.colorSelectorField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPaintToolsColorPickers ColorPickers {
            get {
                return this.colorPickersField;
            }
            set {
                this.colorPickersField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPaintToolsColorSetter {
        
        private RegionType regionField;
        
        /// <remarks/>
        public RegionType Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPaintToolsBrushSizeSelector {
        
        private RegionType regionField;
        
        private ToolboxLayoutDefinitionPaintToolsBrushSizeSelectorBrushSize brushSizeField;
        
        private GaugeType gaugeField;
        
        /// <remarks/>
        public RegionType Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPaintToolsBrushSizeSelectorBrushSize BrushSize {
            get {
                return this.brushSizeField;
            }
            set {
                this.brushSizeField = value;
            }
        }
        
        /// <remarks/>
        public GaugeType Gauge {
            get {
                return this.gaugeField;
            }
            set {
                this.gaugeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPaintToolsBrushSizeSelectorBrushSize {
        
        private int minimumField;
        
        private int maximumField;
        
        private int initialField;
        
        /// <remarks/>
        public int Minimum {
            get {
                return this.minimumField;
            }
            set {
                this.minimumField = value;
            }
        }
        
        /// <remarks/>
        public int Maximum {
            get {
                return this.maximumField;
            }
            set {
                this.maximumField = value;
            }
        }
        
        /// <remarks/>
        public int Initial {
            get {
                return this.initialField;
            }
            set {
                this.initialField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPaintToolsColorSelector {
        
        private RegionType regionField;
        
        private GaugeType gaugeField;
        
        /// <remarks/>
        public RegionType Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
        
        /// <remarks/>
        public GaugeType Gauge {
            get {
                return this.gaugeField;
            }
            set {
                this.gaugeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPaintToolsColorPickers {
        
        private ToolboxLayoutDefinitionPaintToolsColorPickersColorPicker[] colorPickerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ColorPicker")]
        public ToolboxLayoutDefinitionPaintToolsColorPickersColorPicker[] ColorPicker {
            get {
                return this.colorPickerField;
            }
            set {
                this.colorPickerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPaintToolsColorPickersColorPicker {
        
        private RegionType regionField;
        
        /// <remarks/>
        public RegionType Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPlaybackTools {
        
        private ToolboxLayoutDefinitionPlaybackToolsProgressBar progressBarField;
        
        private ToolboxLayoutDefinitionPlaybackToolsSpeedGauge speedGaugeField;
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPlaybackToolsProgressBar ProgressBar {
            get {
                return this.progressBarField;
            }
            set {
                this.progressBarField = value;
            }
        }
        
        /// <remarks/>
        public ToolboxLayoutDefinitionPlaybackToolsSpeedGauge SpeedGauge {
            get {
                return this.speedGaugeField;
            }
            set {
                this.speedGaugeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPlaybackToolsProgressBar {
        
        private RegionType regionField;
        
        private int progresIndicatorWidthField;
        
        private int progressIndicatorHeightField;
        
        /// <remarks/>
        public RegionType Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
        
        /// <remarks/>
        public int ProgresIndicatorWidth {
            get {
                return this.progresIndicatorWidthField;
            }
            set {
                this.progresIndicatorWidthField = value;
            }
        }
        
        /// <remarks/>
        public int ProgressIndicatorHeight {
            get {
                return this.progressIndicatorHeightField;
            }
            set {
                this.progressIndicatorHeightField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ToolboxLayoutDefinitionPlaybackToolsSpeedGauge {
        
        private RegionType regionField;
        
        private GaugeType gaugeField;
        
        /// <remarks/>
        public RegionType Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
        
        /// <remarks/>
        public GaugeType Gauge {
            get {
                return this.gaugeField;
            }
            set {
                this.gaugeField = value;
            }
        }
    }
}
