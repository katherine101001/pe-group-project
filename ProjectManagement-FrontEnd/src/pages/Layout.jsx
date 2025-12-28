import { useState, useEffect } from 'react'
import Navbar from '../components/Navbar'
import Sidebar from '../components/Sidebar'
import { Outlet } from 'react-router-dom'
import { useDispatch } from 'react-redux'
import { loadTheme } from '../features/themeSlice'

const Layout = () => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [searchKeyword, setSearchKeyword] = useState(""); // 搜索关键字状态
    const [refreshKey, setRefreshKey] = useState(0);
    const dispatch = useDispatch();
    const handleRefresh = () => setRefreshKey(prev => prev + 1);

    // 初始加载主题
    useEffect(() => {
        dispatch(loadTheme())
    }, [dispatch])

    return (
        <div className="flex bg-white dark:bg-zinc-950 text-gray-900 dark:text-slate-100">
            <Sidebar
                isSidebarOpen={isSidebarOpen}
                setIsSidebarOpen={setIsSidebarOpen}
                searchKeyword={searchKeyword} // 将搜索关键字传给 Sidebar
                refreshKey={refreshKey}
            />
            <div className="flex-1 flex flex-col h-screen">
                <Navbar
                    isSidebarOpen={isSidebarOpen}
                    setIsSidebarOpen={setIsSidebarOpen}
                    onSearch={setSearchKeyword} // 搜索时更新 searchKeyword
                />
                <div className="flex-1 h-full p-6 xl:p-10 xl:px-16 overflow-y-scroll">
                    <Outlet context={{ refreshKey, handleRefresh }} />
                </div>
            </div>
        </div>
    )
}

export default Layout
