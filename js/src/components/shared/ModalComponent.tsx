import * as React from "react";
import styled from "styled-components";
import styles from "constants/styles" ;

export interface ModalComponentProps {
    onCancelClick(): void;
    title: string
}

const ModalComponent: React.SFC<ModalComponentProps> = (props) => {
    return (
        <OverlayDiv>
            <ModalDiv>
                <HeaderDiv>
                    <span>{props.title}</span>
                    <ClickableIcon className="fa fa-times" onClick={props.onCancelClick} />
                </HeaderDiv>
                <div>
                    {props.children}
                </div>
            </ModalDiv>
        </OverlayDiv>
    );
}

const ClickableIcon = styled.i`
    cursor: pointer;
    position: absolute;
    font-size: 15px;
    right: 9px;
    top: 9px;
`

const OverlayDiv = styled.div`
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    top: 0;
    background: rgba(255,255,255,0.8);
    text-align: center;

    &:before {
        content: '';
        display: inline-block;
        height: 100%;
        vertical-align: middle;
        margin-right: -0.25em; /* Adjusts for spacing */
    }
`

const ModalDiv = styled.div`
    display: inline-block;
    vertical-align: middle;
    background-color: white;
    border-radius: 4px;
    width: 350px;
    box-shadow: 0 0 0 1px rgba(14,41,57,.12), 0 2px 5px rgba(14,41,57,.44), inset 0 -1px 2px rgba(14,41,57,.15);
`

const HeaderDiv = styled.div`
    position: relative;
    text-align: center;
    color: ${styles.colors.fontPrimary};
    background: ${styles.colors.light};
    border-radius: 4px 4px 0 0;

    span {
        font-size: 13px;
        line-height: 35px;
    }

`

export default ModalComponent;



