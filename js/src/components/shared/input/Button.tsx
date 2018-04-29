import * as React from "react";
import styled from "styled-components";
import styles from "constants/styles" ;
import * as classnames from "classnames";

const StyledButton = styled.button`
    height: 42px;
    font-size: 14px;
    border-radius: 5px;
    border: 0;
    text-align: center;
    color: ${styles.colors.fontPrimary};
    background-color: ${styles.colors.light};
    cursor: pointer;
    padding: 0 20px;

    img {
        margin-top: 5px;
        height: 18px;
    }

    &.green {
        background-color: ${styles.colors.green};
    }

    &.red {
        background: ${styles.colors.red};
    }
`

export interface ButtonProps {
    loading?: boolean,
    text: string,
    onClicked: () => void,
    className?: string,
    color?: string
}

const Button: React.SFC<ButtonProps> = (props) => {
    let className = classnames(props.className, props.color);

    return (
        <StyledButton 
            className={className} 
            onClick={(e) => { props.onClicked() }}
        >
                {props.loading && <img src="dist/img/loader.svg" />}
                {!props.loading && <span>{props.text}</span>}
        </StyledButton>
    )
}

export default Button;