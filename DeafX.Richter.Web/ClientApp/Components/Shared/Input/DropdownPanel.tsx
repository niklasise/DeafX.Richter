import * as React from "react";
import classNames from "classnames";

export interface DropdownPanelProps {
    filter: any,
    scrollHeight: number,
    className?: string,
    onClick: (event: any) => any
}

const DropdownPanel: React.SFC<DropdownPanelProps> = (props) => {
    let className = classNames('ui-dropdown-panel ui-widget-content ui-corner-all ui-helper-hidden ui-shadow', props.className);

    return (
        <div className={className} onClick={props.onClick}>
            {this.props.filter}
            <div className="ui-dropdown-items-wrapper" style={{ maxHeight: props.scrollHeight || 'auto' }}>
                <ul className="ui-dropdown-items ui-dropdown-list ui-widget-content ui-widget ui-corner-all ui-helper-reset">
                    {props.children}
                </ul>
            </div>
        </div>
    );
}

export default DropdownPanel;