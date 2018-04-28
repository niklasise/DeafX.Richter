import * as React from "react";

export interface TextInputProps {
    password?: boolean,
    value: string,
    onChange: (target: string, event: any) => void
    icon: string;
    name: string;
    placeholder?: string;
    error?: string;
    additionalClasses?: string,
    disabled?: boolean
}

const TextInput: React.SFC<TextInputProps> = (props) => {
    return <div className="textInput">

        <input
            type={props.password ? "password" : "text"}
            className={!props.error ? props.additionalClasses : "error " + props.additionalClasses}
            value={props.value}
            onChange={(event) => { props.onChange(props.name, event); }}
            placeholder={props.placeholder == null ? "" : props.placeholder}
            disabled={props.disabled}
        />

        {!!props.error && <span>{props.error}</span>}

        {!!props.icon && <i className={"fa " + props.icon} />}
        
    </div>
}

TextInput.defaultProps = {
    additionalClasses: ""
}

export default TextInput;