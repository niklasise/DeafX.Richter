import * as React from "react";
import classNames from "classnames";

export interface DropdownItemProps {
    option: object,
    label: string,
    selected: boolean,
    onClick: ({ event: any, option: object}) => any
}

const DropdownItem: React.SFC<DropdownItemProps> = (props) => {
    let className = classNames('ui-dropdown-item ui-corner-all', {
        'ui-state-highlight': this.props.selected,
        'ui-dropdown-item-empty': (!this.props.label || this.props.label.length === 0)
    });

    return (
        <li className={className} onClick={(e) => { props.onClick({ event: e, option: props.option })}} >
            {props.label}
        </li >
    );
}

export default DropdownItem;