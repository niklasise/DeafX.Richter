import * as React from "react";
import styled from "styled-components";
import styles from "constants/styles" ;
import * as classnames from "classnames";

const StyledDiv = styled.div`
    position: relative;
    margin-bottom: 20px;

    input {
        color: #000;
        width: 100%;
        height: 42px;
        border-radius: 5px;
        border: 1px solid #ccc;
        font-size: 14px;
        padding: 0 50px 0 20px;
        outline: none;

        &.error {
            border-color: ${styles.colors.red};
        }
    }

    i {
        color: ${styles.colors.light};
        position: absolute;
        right: 30px;
        top: 13px;
        font-size: 18px;
    }

    span { 
        display: block;
        color: ${styles.colors.red};
        font-size: 11px;
        margin-top: 3px;
    }
`

export interface TextInputProps {
    password?: boolean,
    value: string,
    onChange: (target: string, event: any) => void
    icon: string;
    name: string;
    placeholder?: string;
    error?: string;
    className?: string,
    disabled?: boolean
}

const TextInput: React.SFC<TextInputProps> = (props) => {
    let className = classnames(props.className, {"error": props.error})
    
    return (
        <StyledDiv className="textInput">
            <input
                type={props.password ? "password" : "text"}
                className={className}
                value={props.value}
                onChange={(event) => { props.onChange(props.name, event); }}
                placeholder={props.placeholder == null ? "" : props.placeholder}
                disabled={props.disabled}
            />

            {!!props.error && <span>{props.error}</span>}

            {!!props.icon && <i className={"fa " + props.icon} />}
            
        </StyledDiv>
    )
}

export default TextInput;