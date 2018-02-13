import * as React from "react";

interface DropdownState {
    isOpen: boolean;
}

interface DropdownProps {
    value?: string,
    onChange?: (target: string, event: any) => void
    name?: string;
    placeholder?: string;
    error?: string;
    className?: string,
    disabled?: boolean
}

export default class Dropdown extends React.Component<DropdownProps, DropdownState> {


    constructor() {
        super();

        this.state = {
            isOpen: true
        }

    }

    public render() {
        return <div className={"dropdown " + this.props.className}>
            <div className={this.state.isOpen ? "box open" : "box"}>
                {!this.props.value && <span className="placeholder">{this.props.placeholder}</span>}
                {!!this.props.value && <span className="value">{this.props.placeholder}</span>}
                <i className="fa fa-caret-down" />
            </div>
            {this.state.isOpen &&
                <div className="options-container">
                    <div className="option">Windows Authentication</div>
                    <div className="option">Basic Auth</div>
                    <div className="option">Open Auth</div>
                </div>
            }
        </div>
    }

}


