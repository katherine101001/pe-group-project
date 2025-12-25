import { useEffect, useRef } from 'react'
import { NavLink } from 'react-router-dom'
import { useSelector } from 'react-redux'
import MyTasksSidebar from './MyTasksSidebar'
import ProjectSidebar from './ProjectsSidebar'
import { FolderOpenIcon, LayoutDashboardIcon, UsersIcon, ArchiveIcon } from 'lucide-react'
import axios from 'axios'
import { toast } from 'react-hot-toast'

const Sidebar = ({ isSidebarOpen, setIsSidebarOpen, searchKeyword, currentProject }) => {
    const { role } = useSelector((state) => state.user ?? {})

    const menuItems = [
        { name: 'Dashboard', href: '/app', icon: LayoutDashboardIcon },
        { name: 'Projects', href: '/app/projects', icon: FolderOpenIcon },
        { name: 'Team', href: '/app/team', icon: UsersIcon },
        { name: 'Archive', href:'/app/archive', icon: ArchiveIcon},
    ]

    const sidebarRef = useRef(null)

    useEffect(() => {
        function handleClickOutside(event) {
            if (sidebarRef.current && !sidebarRef.current.contains(event.target)) {
                setIsSidebarOpen(false)
            }
        }
        document.addEventListener("mousedown", handleClickOutside)
        return () => document.removeEventListener("mousedown", handleClickOutside)
    }, [setIsSidebarOpen])

    const handleArchiveToggle = async () => {
        if (!currentProject) return
        const projectId = currentProject.id
        try {
            if (currentProject.isArchived) {
                await axios.post(`/api/projects/${projectId}/unarchive`)
                toast.success('Project unarchived successfully')
            } else {
                await axios.post(`/api/projects/${projectId}/archive`)
                toast.success('Project archived successfully')
            }
            // 可选：刷新项目数据或状态
        } catch (err) {
            toast.error('Failed to toggle archive status')
            console.error(err)
        }
    }

    return (
        <div
            ref={sidebarRef}
            className={`z-10 bg-white dark:bg-zinc-900 min-w-68 flex flex-col h-screen border-r border-gray-200 dark:border-zinc-800 max-sm:absolute transition-all ${isSidebarOpen ? 'left-0' : '-left-full'}`}
        >
            <hr className='border-gray-200 dark:border-zinc-800' />
            <div className='flex-1 overflow-y-scroll no-scrollbar flex flex-col'>
                <div>
                    <div className='p-4'>
                        {menuItems.map((item) => (
                            <NavLink
                                to={item.href}
                                key={item.name}
                                className={({ isActive }) =>
                                    `flex items-center gap-3 py-2 px-4 text-gray-800 dark:text-zinc-100 cursor-pointer rounded transition-all  
                                    ${isActive ? 'bg-gray-100 dark:bg-zinc-900 dark:bg-gradient-to-br dark:from-zinc-800 dark:to-zinc-800/50 dark:ring-zinc-800' : 'hover:bg-gray-50 dark:hover:bg-zinc-800/60'}`
                                }
                            >
                                <item.icon size={16} />
                                <p className='text-sm truncate'>{item.name}</p>
                            </NavLink>
                        ))}

                        
                        {currentProject && (
                            <button
                                className='flex w-full items-center gap-3 py-2 px-4 text-gray-800 dark:text-zinc-100 cursor-pointer rounded hover:bg-gray-50 dark:hover:bg-zinc-800/60 transition-all mt-2'
                                onClick={handleArchiveToggle}
                            >
                                <ArchiveIcon size={16} />
                                <p className='text-sm truncate'>
                                    {currentProject.isArchived ? 'Unarchive' : 'Archive'}
                                </p>
                            </button>
                        )}
                    </div>

                    {/* 传递 searchKeyword 给子组件 */}
                    <MyTasksSidebar searchKeyword={searchKeyword} />
                    <ProjectSidebar searchKeyword={searchKeyword} />
                </div>
            </div>
        </div>
    )
}

export default Sidebar
