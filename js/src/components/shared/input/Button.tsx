import * as React from "react";

export interface ButtonProps {
    loading?: boolean,
    text: string,
    onClicked: () => void,
    additionalClasses?: string,
    color?: string
}

const Button: React.SFC<ButtonProps> = (props) => {
    return <button className={"btn " + props.color + " " + props.additionalClasses} onClick={(e) => { props.onClicked() }}>
        {props.loading && <img src="dist/img/loader.svg" />}
        {!props.loading && <span>{props.text}</span>}
    </button>
}

export default Button;