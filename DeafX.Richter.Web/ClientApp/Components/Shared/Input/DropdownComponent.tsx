﻿import * as React from "react";
import * as Utils from "../Utils";
import classNames from "classnames";
import DropdownItem from "./DropdownItem";
import DropdownPanel from "./DropdownPanel";

interface DropdownState {
    isOpen: boolean;
}

interface DropdownProps {
    id: string,
    inputId: string,
    value?: string,
    onChange?: (object:any) => void
    name?: string;
    placeholder?: string;
    error?: string;
    className?: string,
    disabled?: boolean
    options: any[]
    optionLabel: string,
    tabIndex: number,
    autoFocus: boolean,
    filter: boolean,
    dataKey: string,
    autoWidth: boolean
    scrollHeight: number;
}

export default class Dropdown extends React.Component<DropdownProps, any> {

    nativeSelect: any;
    focusInput: any;
    documentClickListener: any;
    selfClick: boolean;
    overlayClick: any;
    //editableInputClick: any;
    panel: any;
    filterInput: any;
    expeditableInputClick: any;
    container: any;


    constructor(props) {
        super(props);
        this.state = {
            filter: ''
        };

        this.onClick = this.onClick.bind(this);
        this.onInputFocus = this.onInputFocus.bind(this);
        this.onInputBlur = this.onInputBlur.bind(this);
        this.onInputKeyDown = this.onInputKeyDown.bind(this);
        this.onOptionClick = this.onOptionClick.bind(this);
        this.onFilterInputChange = this.onFilterInputChange.bind(this);
        this.onFilterInputKeyDown = this.onFilterInputKeyDown.bind(this);
        this.panelClick = this.panelClick.bind(this);
    }

    addClass(element, className) {
        if (element.classList)
            element.classList.add(className);
        else
            element.className += ' ' + className;
    }

    removeClass(element, className) {
        if (element.classList)
            element.classList.remove(className);
        else
            element.className = element.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
    }

    onClick(event) {
        if (this.props.disabled) {
            return;
        }

        if (this.documentClickListener) {
            this.selfClick = true;
        }

        if (!this.overlayClick) {
            this.focusInput.focus();

            if (this.panel.element.offsetParent) {
                this.hide();
            }
            else {
                this.show();

                if (this.props.filter) {
                    setTimeout(() => {
                        this.filterInput.focus();
                    }, 200);
                }
            }
        }
    }

    panelClick() {
        this.overlayClick = true;
    }

    onInputFocus(event) {
        this.addClass(this.container, 'ui-state-focus');
    }

    onInputBlur(event) {
        this.removeClass(this.container, 'ui-state-focus');
    }

    onUpKey(event) {
        if (!this.panel.element.offsetParent && event.altKey) {
            this.show();
        }
        else {
            let selectedItemIndex = this.findOptionIndex(this.props.value);

            if (selectedItemIndex !== -1) {
                let nextItemIndex = selectedItemIndex + 1;
                if (nextItemIndex !== (this.props.options.length)) {
                    this.selectItem({
                        originalEvent: event,
                        option: this.props.options[nextItemIndex]
                    });
                }
            }

            if (selectedItemIndex === -1) {
                this.selectItem({
                    originalEvent: event,
                    option: this.props.options[0]
                });
            }
        }

        event.preventDefault();
    }

    onDownKey(event) {
        let selectedItemIndex = this.findOptionIndex(this.props.value);

        if (selectedItemIndex > 0) {
            let prevItemIndex = selectedItemIndex - 1;
            this.selectItem({
                originalEvent: event,
                option: this.props.options[prevItemIndex]
            });
        }

        event.preventDefault();
    }

    onInputKeyDown(event) {
        switch (event.which) {
            //down
            case 40:
                this.onUpKey(event);
                break;

            //up
            case 38:
                this.onDownKey(event);
                break;

            //space
            case 32:
                if (!this.panel.element.offsetParent) {
                    this.show();
                    event.preventDefault();
                }
                break;

            //enter
            case 13:
                this.hide();
                this.unbindDocumentClickListener();
                event.preventDefault();
                break;

            //escape and tab
            case 27:
            case 9:
                this.hide();
                this.unbindDocumentClickListener();
                break;

            default:
                break;
        }
    }

    onOptionClick(event) {
        this.selectItem(event);
        this.focusInput.focus();
        this.hide();
        event.originalEvent.stopPropagation();
    }

    onFilterInputChange(event) {
        this.setState({ filter: event.target.value });
    }

    onFilterInputKeyDown(event) {
        switch (event.which) {
            //down
            case 40:
                this.onUpKey(event);
                break;

            //up
            case 38:
                this.onDownKey(event);
                break;

            //enter
            case 13:
                event.preventDefault();
                break;

            default:
                break;
        }
    }

    selectItem(event) {
        let selectedOption = this.findOption(this.props.value);

        if (selectedOption !== event.option) {
            this.props.onChange({
                originalEvent: event.originalEvent,
                value: this.props.optionLabel ? event.option : event.option.value
            });
        }
    }

    findOptionIndex(value) {
        let index = -1;
        if (this.props.options) {
            for (let i = 0; i < this.props.options.length; i++) {
                let optionValue = this.props.optionLabel ? this.props.options[i] : this.props.options[i].value;
                if ((value === null && optionValue == null) || Utils.equals(value, optionValue, this.props.dataKey)) {
                    index = i;
                    break;
                }
            }
        }

        return index;
    }

    findOption(value) {
        let index = this.findOptionIndex(value);
        return (index !== -1) ? this.props.options[index] : null;
    }

    show() {
        this.panel.element.style.zIndex = "999";
        this.panel.element.style.display = 'block';
        this.bindDocumentClickListener();
    }

    hide() {
        this.panel.element.style.display = 'none';
        this.unbindDocumentClickListener();
        this.clearClickState();
    }

    bindDocumentClickListener() {
        if (!this.documentClickListener) {
            this.documentClickListener = () => {
                if (!this.selfClick && !this.overlayClick) {
                    this.hide();
                }

                this.clearClickState();
            };

            document.addEventListener('click', this.documentClickListener);
        }
    }

    unbindDocumentClickListener() {
        if (this.documentClickListener) {
            document.removeEventListener('click', this.documentClickListener);
            this.documentClickListener = null;
        }
    }

    clearClickState() {
        this.selfClick = false;
        this.overlayClick = false;
    }

    filter(option) {
        let filterValue = this.state.filter.trim().toLowerCase();
        let optionLabel = this.getOptionLabel(option);

        return optionLabel.toLowerCase().indexOf(filterValue.toLowerCase()) > -1;
    }

    hasFilter() {
        return this.state.filter && this.state.filter.trim().length > 0;
    }

    renderHiddenSelect() {
        if (this.props.autoWidth) {
            let options = this.props.options && this.props.options.map((option, i) => {
                return <option key={this.getOptionLabel(option)} value={option.value}>{this.getOptionLabel(option)}</option>;
            });

            return (<div className="ui-helper-hidden-accessible">
                <select ref={(el) => this.nativeSelect = el} tabIndex={-1} aria-hidden="true">
                    {options}
                </select>
            </div>);
        }
        else {
            return null;
        }
    }

    renderKeyboardHelper() {
        return <div className="ui-helper-hidden-accessible">
            <input ref={(el) => this.focusInput = el} id={this.props.inputId} type="text" role="listbox"
                onFocus={this.onInputFocus} onBlur={this.onInputBlur} onKeyDown={this.onInputKeyDown}
                disabled={this.props.disabled} tabIndex={this.props.tabIndex} autoFocus={this.props.autoFocus} />
        </div>;
    }

    renderLabel(label) {
        let className = classNames('ui-dropdown-label ui-inputtext ui-corner-all', {
            'ui-placeholder': label === null && this.props.placeholder,
            'ui-dropdown-label-empty': label === null && !this.props.placeholder
        }
        );

        return <label className={className}>{label || this.props.placeholder || 'empty'}</label>;
    }

    renderDropdownIcon() {
        return <div className="ui-dropdown-trigger ui-state-default ui-corner-right">
            <span className="fa fa-fw fa-caret-down ui-clickable"></span>
        </div>;
    }

    renderItems(selectedOption) {
        let items = this.props.options;

        if (items && this.hasFilter()) {
            items = items && items.filter((option) => {
                return this.filter(option);
            });
        }

        if (items) {
            return items.map((option, index) => {
                let optionLabel = this.getOptionLabel(option);
                return <DropdownItem key={optionLabel} label={optionLabel} option={option} template={this.props.itemTemplate} selected={selectedOption === option}
                    onClick={this.onOptionClick} />;
            });
        }
        else {
            return null;
        }
    }

    renderFilter() {
        if (this.props.filter) {
            return <div className="ui-dropdown-filter-container">
                <input ref={(el) => this.filterInput = el} type="text" autoComplete="off" className="ui-dropdown-filter ui-inputtext ui-widget ui-state-default ui-corner-all" placeholder={this.props.filterPlaceholder}
                    onKeyDown={this.onFilterInputKeyDown} onChange={this.onFilterInputChange} />
                <span className="fa fa-search"></span>
            </div>;
        }
        else {
            return null;
        }
    }

    getOptionLabel(option) {
        return this.props.optionLabel ? Utils.retreiveObjectFieldData(option, this.props.optionLabel) : option.label;
    }

    componentWillUnmount() {
        this.unbindDocumentClickListener();
    }

    componentDidUpdate(prevProps, prevState) {
        if (this.panel.element.offsetParent) {
            let highlightItem = DomHandler.findSingle(this.panel.element, 'li.ui-state-highlight');
            if (highlightItem) {
                DomHandler.scrollInView(this.panel.itemsWrapper, highlightItem);
            }
        }
    }

    render() {
        let className = classNames('ui-dropdown ui-widget ui-state-default ui-corner-all ui-helper-clearfix', this.props.className, { 'ui-state-disabled': this.props.disabled });
        let selectedOption = this.findOption(this.props.value);
        let label = selectedOption ? this.getOptionLabel(selectedOption) : null;

        let hiddenSelect = this.renderHiddenSelect();
        let keyboardHelper = this.renderKeyboardHelper();
        let labelElement = this.renderLabel(label);
        let dropdownIcon = this.renderDropdownIcon();
        let items = this.renderItems(selectedOption);
        let filterElement = this.renderFilter();

        return (
            <div id={this.props.id} ref={(el) => this.container = el} className={className} onClick={this.onClick}>
                {hiddenSelect}
                {keyboardHelper}
                {labelElement}
                {dropdownIcon}
                <DropdownPanel ref={(el) => this.panel = el}
                    scrollHeight={this.props.scrollHeight} onClick={this.panelClick} filter={filterElement}>
                    {items}
                </DropdownPanel>
            </div>
        );
    }


}


//constructor() {
//    super();

//    this.state = {
//        isOpen: true
//    }


//}

//onInputFocus(event) {
//    DomHandler.addClass(this.container, 'ui-state-focus');
//}

//onInputBlur(event) {
//    DomHandler.removeClass(this.container, 'ui-state-focus');
//}

//onUpKey(event) {
//    if (!this.panel.element.offsetParent && event.altKey) {
//        this.show();
//    }
//    else {
//        let selectedItemIndex = this.findOptionIndex(this.props.value);

//        if (selectedItemIndex !== -1) {
//            let nextItemIndex = selectedItemIndex + 1;
//            if (nextItemIndex !== (this.props.options.length)) {
//                this.selectItem({
//                    originalEvent: event,
//                    option: this.props.options[nextItemIndex]
//                });
//            }
//        }

//        if (selectedItemIndex === -1) {
//            this.selectItem({
//                originalEvent: event,
//                option: this.props.options[0]
//            });
//        }
//    }

//    event.preventDefault();
//}

//onDownKey(event) {
//    let selectedItemIndex = this.findOptionIndex(this.props.value);

//    if (selectedItemIndex > 0) {
//        let prevItemIndex = selectedItemIndex - 1;
//        this.selectItem({
//            originalEvent: event,
//            option: this.props.options[prevItemIndex]
//        });
//    }

//    event.preventDefault();
//}

//onInputKeyDown(event) {
//    switch (event.which) {
//        //down
//        case 40:
//            this.onUpKey(event);
//            break;

//        //up
//        case 38:
//            this.onDownKey(event);
//            break;

//        //space
//        case 32:
//            if (!this.panel.element.offsetParent) {
//                this.show();
//                event.preventDefault();
//            }
//            break;

//        //enter
//        case 13:
//            this.hide();
//            this.unbindDocumentClickListener();
//            event.preventDefault();
//            break;

//        //escape and tab
//        case 27:
//        case 9:
//            this.hide();
//            this.unbindDocumentClickListener();
//            break;

//        default:
//            break;
//    }
//}

//getOptionLabel(option: any): string {
//    return this.props.optionLabel ? Utils.retreiveObjectFieldData(option, this.props.optionLabel) : option.label;
//}

//renderHiddenSelect() : JSX.Element {
//    let options = this.props.options && this.props.options.map((option, i) => {
//        return <option key={this.getOptionLabel(option)} value={option.value}>{this.getOptionLabel(option)}</option>;
//    });

//    return (<div className="ui-helper-hidden-accessible">
//        <select ref={(el) => this.nativeSelect = el} tabIndex={-1} aria-hidden="true">
//            {options}
//        </select>
//    </div>);
//}

//renderKeyboardHelper() {
//    return <div className="ui-helper-hidden-accessible">
//        <input ref={(el) => this.focusInput = el} id={this.props.inputId} type="text" role="listbox"
//            onFocus={this.onInputFocus} onBlur={this.onInputBlur} onKeyDown={this.onInputKeyDown}
//            disabled={this.props.disabled} tabIndex={this.props.tabIndex} autoFocus={this.props.autoFocus} />
//    </div>;
//}

//    public render() {
//    let hiddenSelect = this.renderHiddenSelect();
//    let keyboardHelper = this.renderKeyboardHelper();

//    return <div id={this.props.id} className={"dropdown " + this.props.className}>
//        {hiddenSelect}
//        {keyboardHelper}
//        {/*<div className={this.state.isOpen ? "box open" : "box"}>
//                {!this.props.value && <span className="placeholder">{this.props.placeholder}</span>}
//                {!!this.props.value && <span className="value">{this.props.placeholder}</span>}
//                <i className="fa fa-caret-down" />
//            </div>*/}
//        {/*this.state.isOpen &&
//                <div className="options-container">
//                    <div className="option">Windows Authentication</div>
//                    <div className="option">Basic Auth</div>
//                    <div className="option">Open Auth</div>
//                </div>
//            */}
//    </div>
//}