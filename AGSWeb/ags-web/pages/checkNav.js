
import React, { useState } from 'react';
import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
    NavbarText,
    Dropdown,
    UncontrolledDropdown
} from 'reactstrap';

export default function CheckNav({ }) {
    const [isOpen, setIsOpen] = useState(false);
    const toggle = () => setIsOpen(!isOpen);

    return (
        <div>
            <div>
            <Navbar color="light" light expand="md">
                <NavbarBrand href="/">AGS</NavbarBrand>
                <NavbarToggler onClick={toggle} />
                <Collapse isOpen={isOpen} navbar>
                    <Nav className="mr-auto" navbar>
                <UncontrolledDropdown direction="down">
                    <DropdownToggle caret>
                        Drop Down
                    </DropdownToggle>
                    <DropdownMenu>
                        <DropdownItem>
                            Action 1
                        </DropdownItem>
                        <DropdownItem>
                            Action 1
                        </DropdownItem>
                        <DropdownItem>
                            Action 1
                        </DropdownItem>
                        <DropdownItem>
                            Action 2
                        </DropdownItem>
                        <SubDropdownItem level={20}/>
                    </DropdownMenu>
                </UncontrolledDropdown>
                </Nav></Collapse></Navbar>
            </div>
        </div>
    )
}

function SubDropdownItem({level}){
    return (
        <UncontrolledDropdown direction="right">
            <DropdownToggle caret>
                Drop Down
            </DropdownToggle>
            <DropdownMenu>
            <DropdownItem>
                    Action 1
                </DropdownItem>
                <DropdownItem>
                    Action 2
                </DropdownItem>
                <DropdownItem>
                    Action 1
                </DropdownItem>
                <DropdownItem>
                    Action 2
                </DropdownItem>
                { level == 0 ? (
                    <div></div>
                ) : (
                    <SubDropdownItem level={level-1}></SubDropdownItem>
                )}
            </DropdownMenu>
        </UncontrolledDropdown>
    )
}