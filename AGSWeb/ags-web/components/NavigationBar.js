import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
    UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
    NavbarText
} from 'reactstrap'
import React, { useState } from 'react';
 

function NavigationItem({ menuOption, level }) {
    if (menuOption.ChildrenMenus == undefined || menuOption.ChildrenMenus.length == 0) {
        if (level == 0) {
            return (
                <NavItem>
                    <NavLink href={menuOption.Url}>{menuOption.LabelKey}</NavLink>
                </NavItem>
            )
        } else {
            return (
                <DropdownItem><a href={menuOption.Url}>{menuOption.LabelKey}</a></DropdownItem>
            )
        }
    }
 
    if (menuOption.ChildrenMenus != undefined && menuOption.ChildrenMenus.length > 0) {
        return (
            <UncontrolledDropdown inNavbar direction={level == 0 ? "down" : "right"}>
                <DropdownToggle nav caret>
                    {menuOption.LabelKey}
                </DropdownToggle>
                <DropdownMenu>
                    {
                        menuOption.ChildrenMenus.map(x => {
                            return (
                                <NavigationItem level={level + 1} menuOption={x} />
                            )
                        })
                    }
                </DropdownMenu>
            </UncontrolledDropdown>
        )
    }
}
 
export default function NavigationBar({ menuOptions }) {
    const [isOpen, setIsOpen] = useState(false);
    const toggle = () => setIsOpen(!isOpen);
 
    return (
        <Navbar expand="md" color="light" light>
            <NavbarBrand><h1>AGS</h1></NavbarBrand>
            <NavbarToggler onClick={toggle} />
            <Collapse isOpen={isOpen} navbar>
                <Nav className="mr-auto" navbar>
                    {
                        menuOptions.map(x => {
                            return (
                                <NavigationItem menuOption={x} level={0} />
                            )
                        })
                    }
                </Nav>
                <NavbarText>Logout</NavbarText>
            </Collapse>
        </Navbar>
    )
}