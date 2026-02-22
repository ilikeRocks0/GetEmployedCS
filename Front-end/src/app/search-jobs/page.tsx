import NavigationBar from "@/components/navigationBar"

export default function SearchPage()
{
    return(
    <div className="h-screen">

        <body>
            <NavigationBar>
            </NavigationBar>
        </body>
        {/* Search Bar */}
        <div className="grid gap-0">
            <input className="h-10 w-xl mt-10 justify-self-center text-center border" placeholder="Search"/>
        </div>

        {/* Filters */}
        <div className="flex w-full p-5">
            <div className="grow"/>
            <button className="border m-5 w-50 p-2">Job Type \/</button>
            <button className="border m-5 w-50 p-2">Job Position \/</button>
            <button className="border m-5 w-50 p-2">Language \/</button>
            <div className="grow"/>
        </div>
        
        {/* Job List */}
        <div className="mt-15 justify-items-center h-full">
            <div className="grid grid-cols-4 m-1 mx-30 h-30 w-400 border">
                <p className="text-5xl justify-self-center m-auto font-bold">Canada Hydro</p>
                <p className="m-auto">Language: React</p>
                <p className="m-auto">Job Position: Front-End</p>
                <p className="m-auto">Job Type: Internship</p>
            </div>
            <div className="grid grid-cols-4 m-1 mx-30 h-30 w-400 border">
                <p className="text-5xl justify-self-center m-auto font-bold">Canada Hydro</p>
                <p className="m-auto">Language: React</p>
                <p className="m-auto">Job Position: Front-End</p>
                <p className="m-auto">Job Type: Internship</p>
            </div>
            <div className="grid grid-cols-4 m-1 mx-30 h-30 w-400 border">
                <p className="text-5xl justify-self-center m-auto font-bold">Canada Hydro</p>
                <p className="m-auto">Language: React</p>
                <p className="m-auto">Job Position: Front-End</p>
                <p className="m-auto">Job Type: Internship</p>
            </div>
            <div className="grid grid-cols-4 m-1 mx-30 h-30 w-400 border">
                <p className="text-5xl justify-self-center m-auto font-bold">Canada Hydro</p>
                <p className="m-auto">Language: React</p>
                <p className="m-auto">Job Position: Front-End</p>
                <p className="m-auto">Job Type: Internship</p>
            </div>

        </div>


    </div>
    )
}